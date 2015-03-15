using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UAVT.Utils;
using UAVT_Exporter.Model;
using UAVT_Exporter.SQLLite;

namespace UAVT_Exporter {
    public partial class Form1 : Form {

        private string _selectedCode;
        private Configuration _selectedItem;
        private string _sourceFileName;
        private string _sourceFilePath;
        private bool _isDateSelected;
        private string _wsUrl = "http://10.34.61.33/UAVTWebapi/";
        //private string _wsUrl = "http://localhost/UAVTWebapi/";
        private ILog _log = LogManager.GetLogger(typeof(Form1));

        private static List<Configuration> _configurations=new List<Configuration>(); 

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender,EventArgs e)
        {

            cmbCities.DataSource = City.GetCities();
            cmbCities.DisplayMember = "CityName";
            cmbCities.ValueMember = "CityCode";

            cmbDistricts.Enabled = false;

            bckWorker.WorkerReportsProgress = true;

            Task.Factory.StartNew(() => {
                if(!Utilities.CheckWebServiceExist(_wsUrl + "api/v1/uavt/IsAlive")) {
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).WarnFormat("Unable to connect server address [{0}]",_wsUrl);
                    _wsUrl = "http://uavt.dedas.com.tr/UAVTWebapi/";
                    //wsUrl = "http://localhost/UAVTWebapi/";
                    if(!Utilities.CheckWebServiceExist(_wsUrl + "api/v1/uavt/IsAlive")) {
                        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).WarnFormat("Unable to connect server address [{0}]",_wsUrl);
                        _wsUrl = "http://localhost/UAVTWebapi/";
                    }
                }

                rdStat.BeginInvoke((Action)(() => {
                    rdStat.Checked = true;
                    rdStat.Text = "Bağlı";
                }));

                lblMessage.BeginInvoke((Action)(() => {
                    lblMessage.Text = "Konfigurasyon verileri çekiliyor.";
                }));

                _configurations = GetConfigurations();

                lblMessage.BeginInvoke((Action)(() => {
                    lblMessage.Text = "Konfigurasyon verileri tamamlandı.";
                }));
                btnStart.BeginInvoke((Action)(() => {
                    btnStart.Enabled = true;
                }));
                cmbDistricts.BeginInvoke((Action)(() => {
                    cmbDistricts.Enabled = true;
                }));

                btnStart.BeginInvoke((Action)(() => {
                    btnStart.Enabled = true;
                }));
            });
        }

        private void btnOpenFDSource_Click(object sender,EventArgs e) {
            ofDialog = new OpenFileDialog();
            if(ofDialog.ShowDialog() == DialogResult.OK) {
                txtSourcePath.Text = ofDialog.FileName;
                _sourceFileName = ofDialog.FileName;
                var sf = ofDialog.SafeFileName;
                _sourceFilePath = _sourceFileName.Substring(0,_sourceFileName.IndexOf(sf));
                _sourceFilePath = _sourceFilePath.Substring(0,_sourceFilePath.Length - 1);
            }
        }

        private void cmbDistricts_SelectedIndexChanged(object sender,EventArgs e) {
            if(cmbDistricts.SelectedItem != null) {
                _selectedCode = (cmbDistricts.SelectedItem as Configuration).MappedDistrictCode;
                _selectedItem = cmbDistricts.SelectedItem as Configuration;
            }
        }

        private void cmbCities_SelectedIndexChanged(object sender,EventArgs e)
        {
            var cityConfigurations = _configurations.Where(a => a.CityCode == cmbCities.SelectedValue.ToString()).ToList();
            cmbDistricts.DataSource = cityConfigurations;
            cmbDistricts.DisplayMember = "CountyName";
            cmbDistricts.Enabled = true;
        }

        private void btnOpenFDDest_Click(object sender,EventArgs e) {
            //reserved for future
        }

        private void btnStart_Click(object sender,EventArgs e) {

            SqlConversionHandler handler = (delegate(bool done,bool success,int percent,string msg) {
                Invoke(new MethodInvoker(delegate() {
                    lblMessage.Text = msg;
                    pbStatus.Value = percent;
                    lblCount.Text = percent.ToString();
                    if(done) {
                        btnStart.Enabled = true;
                        if(success) {
                            MessageBox.Show(this,
                                   msg,
                                   "Oluşturma Tamamlandı",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                            pbStatus.Value = 0;
                            lblMessage.Text = string.Empty;
                            lblCount.Text = string.Empty;
                        }
                    }
                }));
            });

            var destinationFile = Utilities.CreateOrUseExistedFolder(_selectedItem.CountyName,_sourceFilePath);
            SQLiteCreator.CreateSqliteDb(destinationFile,_sourceFileName,_configurations,
                _selectedItem,GetNewUavtData(),GetUavtData(),GetMbsData(),GetAuditLogData(),GetUsers(),handler);

            //bckWorker.RunWorkerAsync();
            btnStart.Enabled = false;
        }

        private List<Uavts> GetUavtData() {
            using(var client = new HttpClient()) {
                string list = "";
                client.BaseAddress = new Uri(_wsUrl);
                client.Timeout = new TimeSpan(0,0,10,0);
                var resp = client.PostAsJsonAsync("api/v1/uavt/fetch",new FetchRequestModel { DistrictCode = _selectedCode,LastProcessDate = null });
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).InfoFormat("get uavt data called date=[{0}]",DateTime.Now);


                resp.ContinueWith((a) => {
                    log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).InfoFormat("get uavt data result resp came date=[{0}]",DateTime.Now);
                    var stringTask = a.Result.Content.ReadAsStringAsync();
                    stringTask.ContinueWith((b) => {
                        log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).InfoFormat("get uavt data result resp string came date=[{0}]",DateTime.Now);
                        list = b.Result;
                    }).Wait();
                }).Wait();

                var respResult = JsonConvert.DeserializeObject(list,typeof(List<Uavts>)) as List<Uavts>;
                log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).InfoFormat("get uavt data and deserialized called date=[{0}]",DateTime.Now);

                return respResult;
            }
        }

        private List<AuditLogModel> GetAuditLogData() {
            using(var client = new HttpClient()) {
                string list = "";
                client.BaseAddress = new Uri(_wsUrl);
                client.Timeout = new TimeSpan(0,0,10,0);
                var resp = client.PostAsJsonAsync("api/v1/uavt/fetchLogs",new FetchRequestModel { DistrictCode = _selectedCode,LastProcessDate = null });
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).InfoFormat("get GetAuditLogData data called date=[{0}]",DateTime.Now);


                resp.ContinueWith((a) => {
                    log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).InfoFormat("get GetAuditLogData data result resp came date=[{0}]",DateTime.Now);
                    var stringTask = a.Result.Content.ReadAsStringAsync();
                    stringTask.ContinueWith((b) => {
                        log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).InfoFormat("get GetAuditLogData data result resp string came date=[{0}]",DateTime.Now);
                        list = b.Result;
                    }).Wait();
                }).Wait();

                var respResult = JsonConvert.DeserializeObject(list,typeof(List<AuditLogModel>)) as List<AuditLogModel>;
                log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).InfoFormat("get GetAuditLogData data and deserialized called date=[{0}]",DateTime.Now);

                return respResult;
            }
        }

        private List<DistrictValue> GetNewUavtData() {
            using(var client = new HttpClient()) {
                client.BaseAddress = new Uri(_wsUrl);
                var resp = client.PostAsJsonAsync("api/v1/uavt/fetchNew",new FetchRequestModel { DistrictCode = _selectedItem.MappedDistrictCode,LastProcessDate = null });
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var listString = resp.Result.Content.ReadAsStringAsync().Result;
                var respResult = JsonConvert.DeserializeObject(listString,typeof(List<DistrictValue>)) as List<DistrictValue>;
                return respResult;
            }
        }

        private List<Users> GetUsers() {
            using(var client = new HttpClient()) {
                client.BaseAddress = new Uri(_wsUrl);
                var resp = client.PostAsJsonAsync("api/v1/uavt/fetchUsers",new FetchRequestModel { DistrictCode = _selectedItem.MappedDistrictCode,LastProcessDate = null });
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var listString = resp.Result.Content.ReadAsStringAsync().Result;
                var respResult = JsonConvert.DeserializeObject(listString,typeof(List<Users>)) as List<Users>;
                return respResult;
            }
        }

        private List<SubscriberModel> GetMbsData() {
            using(var client = new HttpClient()) {
                client.BaseAddress = new Uri(_wsUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var fetchModel = new FetchRequestModel { DistrictCode = _selectedCode,LastProcessDate = null };
                if(_isDateSelected) {
                    fetchModel.LastProcessDate = 20141210000000;
                }
                var resp = client.PostAsJsonAsync("api/v1/uavt/fetchMbs",fetchModel);

                var listString = resp.Result.Content.ReadAsStringAsync().Result;
                var respResult = JsonConvert.DeserializeObject(listString,typeof(List<SubscriberModel>)) as List<SubscriberModel>;
                return respResult;
            }
        }

        private List<Configuration> GetConfigurations()
        {
            using (var client=new HttpClient())
            {
                client.BaseAddress=new Uri(_wsUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var resp = client.GetAsync("api/v1/uavt/getConfigurations");
                var listString = resp.Result.Content.ReadAsStringAsync().Result;
                var respResult = JsonConvert.DeserializeObject(listString,typeof(List<Configuration>)) as List<Configuration>;
                return respResult;
            }
        }

        private void chDate_CheckedChanged(object sender,EventArgs e) {
            if(chDate.Checked) {
                _isDateSelected = true;
            }
        }

        private void btnCancel_Click(object sender,EventArgs e) {
            Application.Exit();
        }

    }

   
}
