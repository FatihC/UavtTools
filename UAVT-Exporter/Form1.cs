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
using UAVT_Exporter.SQLLite;

namespace UAVT_Exporter {
    public partial class Form1 : Form {

        private string selectedCode;
        private District selectedItem;
        private string sourceFileName;
        private string sourceFilePath;
        private bool isDateSelected = false;
        private string wsUrl = "http://10.34.61.33/UAVTWebapiTest/";
        private ILog _log = LogManager.GetLogger(typeof(Form1));

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender,EventArgs e) {
            var d = new District();
            var list = d.GetDistricts();

            cmbDistricts.DataSource = list;
            cmbDistricts.DisplayMember = "Name";

            bckWorker.WorkerReportsProgress = true;
            bckWorker.ProgressChanged += bckWorker_ProgressChanged;

            Task.Factory.StartNew(() => {
                if(!Utilities.CheckWebServiceExist(wsUrl + "api/v1/uavt/IsAlive")) {
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).WarnFormat("Unable to connect server address [{0}]",wsUrl);
                    wsUrl = "http://uavt.dedas.com.tr/UAVTWebapi/";
                    //wsUrl = "http://localhost/UAVTWebapi/";
                    if(!Utilities.CheckWebServiceExist(wsUrl + "api/v1/uavt/IsAlive")) {
                        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).WarnFormat("Unable to connect server address [{0}]",wsUrl);
                        wsUrl = "http://localhost/UAVTWebapi/";
                    }
                }

                rdStat.BeginInvoke((Action)(() => {
                    rdStat.Checked = true;
                    rdStat.Text = "Bağlı";
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
                sourceFileName = ofDialog.FileName;
                var sf = ofDialog.SafeFileName;
                sourceFilePath = sourceFileName.Substring(0,sourceFileName.IndexOf(sf));
                sourceFilePath = sourceFilePath.Substring(0,sourceFilePath.Length - 1);
            }
        }

        private void cmbDistricts_SelectedIndexChanged(object sender,EventArgs e) {
            if(cmbDistricts.SelectedItem != null) {
                selectedCode = (cmbDistricts.SelectedItem as District).Code;
                selectedItem = cmbDistricts.SelectedItem as District;
            }
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

            var destinationFile = Utilities.CreateOrUseExistedFolder(selectedItem.Name,sourceFilePath);
            SQLiteCreator.CreateSqliteDb(destinationFile,sourceFileName,Utilities.GetDistricts(),selectedItem,GetNewUavtData(),GetUavtData(),GetMbsData(),GetAuditLogData(),handler);

            //bckWorker.RunWorkerAsync();
            btnStart.Enabled = false;
        }

        private List<Uavts> GetUavtData() {
            using(var client = new HttpClient()) {
                string list = "";
                client.BaseAddress = new Uri(wsUrl);
                client.Timeout = new TimeSpan(0,0,10,0);
                var resp = client.PostAsJsonAsync("api/v1/uavt/fetch",new FetchRequestModel { DistrictCode = selectedCode,LastProcessDate = null });
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).InfoFormat("get uavt data called date=[{0}]",DateTime.Now);


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
                client.BaseAddress = new Uri(wsUrl);
                client.Timeout = new TimeSpan(0,0,10,0);
                var resp = client.PostAsJsonAsync("api/v1/uavt/fetchLogs",new FetchRequestModel { DistrictCode = selectedCode,LastProcessDate = null });
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
                client.BaseAddress = new Uri(wsUrl);
                var resp = client.PostAsJsonAsync("api/v1/uavt/fetchNew",new FetchRequestModel { DistrictCode = selectedItem.DbCode,LastProcessDate = null });
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var listString = resp.Result.Content.ReadAsStringAsync().Result;
                var respResult = JsonConvert.DeserializeObject(listString,typeof(List<DistrictValue>)) as List<DistrictValue>;
                return respResult;
            }
        }

        private List<ABONE_BILGI> GetMbsData() {
            using(var client = new HttpClient()) {
                client.BaseAddress = new Uri(wsUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var fetchModel = new FetchRequestModel { DistrictCode = selectedCode,LastProcessDate = null };
                if(isDateSelected) {
                    fetchModel.LastProcessDate = 20141210000000;
                }
                var resp = client.PostAsJsonAsync("api/v1/uavt/fetchMbs",fetchModel);

                var listString = resp.Result.Content.ReadAsStringAsync().Result;
                var respResult = JsonConvert.DeserializeObject(listString,typeof(List<ABONE_BILGI>)) as List<ABONE_BILGI>;
                return respResult;
            }
        }

        private void bckWorker_DoWork(object sender,DoWorkEventArgs e) {
            var destinationFile = Utilities.CreateOrUseExistedFolder(selectedItem.Name,sourceFilePath);

            if(File.Exists(destinationFile)) {
                File.Delete(destinationFile);
            }
            File.Copy(sourceFileName,destinationFile);


            //getting districts
            var d = new District();
            var distList = d.GetDistricts();


            var connStr = "data source=" + destinationFile;
            using(var conn = new SQLiteConnection(connStr)) {

                conn.Open();
                var cmd = new SQLiteCommand(conn);
                var helper = new SQLiteHelper(cmd);

                //dropping tables
                foreach(var district in distList) {
                    if(selectedCode != district.Code) {
                        var tableName = district.TableName;
                        var tableMBSName = district.TableName + "_MBS";
                        helper.DropTable(tableName);
                        helper.DropTable(tableMBSName);
                    }
                }

                //getting uavt items for specified district
                var dt = helper.Select(String.Format("SELECT * FROM {0}",selectedItem.TableName));
                var districtItemList = new List<DistrictValue>();
                foreach(DataRow rowItem in dt.Rows) {
                    var item = new DistrictValue();
                    item.Id = rowItem.ItemArray[0].ToString();
                    item.CountyCode = rowItem.ItemArray[1].ToString();
                    item.DistrictCode = rowItem.ItemArray[2].ToString();
                    item.DistrictName = rowItem.ItemArray[3].ToString();
                    item.VillageCode = rowItem.ItemArray[4].ToString();
                    item.VillageName = rowItem.ItemArray[5].ToString();
                    item.StreetCode = rowItem.ItemArray[6].ToString();
                    item.StreetName = rowItem.ItemArray[7].ToString();
                    item.CSBMCode = rowItem.ItemArray[8].ToString();
                    item.CSBMName = rowItem.ItemArray[9].ToString();
                    item.BuildingCode = rowItem.ItemArray[10].ToString();
                    item.DoorNumber = rowItem.ItemArray[11].ToString();
                    item.SiteName = rowItem.ItemArray[12].ToString();
                    item.BlockName = rowItem.ItemArray[13].ToString();
                    item.UAVTAddressNo = rowItem.ItemArray[14].ToString();
                    item.IndoorNumber = rowItem.ItemArray[15].ToString();
                    item.CheckStatus = rowItem.ItemArray[16].ToString();
                    districtItemList.Add(item);
                }

                var newUavts = GetNewUavtData();
                if(newUavts == null) {
                    goto UAVT;
                }
                if(newUavts.Count < 1) {
                    goto UAVT;
                }
                int i = 1;
                try {
                    foreach(var districtValue in newUavts) {
                        if(districtItemList.Any(a => a.UAVTAddressNo == districtValue.UAVTAddressNo)) {
                            //item var
                            continue;
                        }

                        var sb = new StringBuilder();
                        sb.Append(String.Format("INSERT INTO {0}",selectedItem.TableName));
                        sb.Append(
                            "(COUNTY_CODE," +
                            "DISTRICT_CODE," +
                            "DISTRICT_NAME," +
                            "VILLAGE_CODE," +
                            "VILLAGE_NAME," +
                            "STREET_CODE,STREET_NAME," +
                            "CSBM_CODE,CSBM_NAME," +
                            "BUILDING_CODE,DOOR_NUMBER," +
                            "SITE_NAME,BLOCK_NAME," +
                            "UAVT_ADDRESS_NO," +
                            "INDOOR_NUMBER," +
                            "CHECK_STATUS)");
                        sb.Append(
                            String.Format(
                                " VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}',{15})",
                                districtValue.CountyCode,
                                districtValue.DistrictCode,
                                districtValue.DistrictName,
                                districtValue.VillageCode,
                                districtValue.VillageName,
                                districtValue.StreetCode,
                                districtValue.StreetName,
                                districtValue.CSBMCode,
                                districtValue.CSBMName,
                                districtValue.BuildingCode,
                                districtValue.DoorNumber,
                                districtValue.SiteName,
                                districtValue.BlockName,
                                districtValue.UAVTAddressNo,
                                districtValue.IndoorNumber,
                                districtValue.CheckStatus
                                ));
                        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                        helper.Execute(sb.ToString());

                        bckWorker.ReportProgress(i);
                        i++;
                    }
                } catch(Exception exc) {
                    log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("Hata oluştu {0}",JsonConvert.SerializeObject(exc,Formatting.Indented));
                }


                //inserting or updating to tables
            UAVT:
                var uavtList = GetUavtData();
                if(uavtList == null) {
                    goto devam;
                }
                i = 1;
                helper.BeginTransaction();
                try {
                    foreach(var uavtse in uavtList) {
                        if(districtItemList.Any(a => a.UAVTAddressNo == uavtse.uavtCode)) {
                            //böyle item var ise
                            var sb = new StringBuilder();
                            sb.Append(String.Format("UPDATE {0} SET CHECK_STATUS={1} WHERE UAVT_ADDRESS_NO='{2}'",selectedItem.TableName,"7",uavtse.uavtCode));
                            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                            helper.Execute(sb.ToString());
                        } else {
                            //yoksa yeni eklenmiştir
                            var itemForVal =
                                districtItemList.FirstOrDefault(
                                    a => a.CSBMCode == uavtse.csbmCode && a.StreetCode == uavtse.streetCode);
                            if(itemForVal == null) {
                                log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType).WarnFormat("There is no such an item with csbmCode {0} and streetCode {1}",uavtse.csbmCode,uavtse.streetCode);
                                continue;
                            }
                            var sb = new StringBuilder();
                            sb.Append(String.Format("INSERT INTO {0}",selectedItem.TableName));
                            sb.Append(
                                "(COUNTY_CODE," +
                                "DISTRICT_CODE," +
                                "DISTRICT_NAME," +
                                "VILLAGE_CODE," +
                                "VILLAGE_NAME," +
                                "STREET_CODE,STREET_NAME," +
                                "CSBM_CODE,CSBM_NAME," +
                                "BUILDING_CODE,DOOR_NUMBER," +
                                "SITE_NAME,BLOCK_NAME," +
                                "UAVT_ADDRESS_NO," +
                                "INDOOR_NUMBER,CHECK_STATUS)");
                            sb.Append(
                                String.Format(
                                    " VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}',{15})",
                                    selectedItem.DbCode,
                                    selectedItem.DbDistrictCode,
                                    itemForVal.DistrictName,
                                    uavtse.villageCode,
                                    itemForVal.VillageName,
                                    uavtse.streetCode,
                                    itemForVal.StreetName,
                                    uavtse.csbmCode,
                                    itemForVal.CSBMName,
                                    itemForVal.BuildingCode,
                                    uavtse.doorNumber,
                                    itemForVal.SiteName,
                                    itemForVal.BlockName,
                                    uavtse.uavtCode,
                                    uavtse.indoorNumber,
                                    "7"
                                    ));
                            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                            helper.Execute(sb.ToString());
                        }
                        bckWorker.ReportProgress(i);
                        i++;
                    }
                    helper.Commit();

                } catch(Exception exc) {
                    helper.Rollback();
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("Hata oluştu {0}",JsonConvert.SerializeObject(exc,Formatting.Indented));
                }

                helper.BeginTransaction();
                try {
                    //aynı uavt listesini push_request'e yaz
                    foreach(var uavtse in uavtList) {
                        var sb = new StringBuilder();
                        sb.Append(String.Format("INSERT INTO PUSH_REQUEST(WIRING_NO" +
                                                ",METER_NO," +
                                                "EXIST_ON_UAVT," +
                                                "CUSTOMER_NAME," +
                                                "CHECK_STATUS," +
                                                "UAVT_CODE," +
                                                "PUSHED," +
                                                "USER_SERNO," +
                                                "DISTRICT_CODE," +
                                                "CREATE_DATE," +
                                                "METER_BRAND," +
                                                "DOOR_NUMBER," +
                                                "VILLAGE_CODE," +
                                                "STREET_CODE," +
                                                "CSBM_CODE," +
                                                "INDOOR_NUMBER," +
                                                "SITE_NAME," +
                                                "BLOCK_NAME," +
                                                "METER_BRAND_CODE" +
                                                ") VALUES ('{0}','{1}',{2},'{3}','{4}','{5}',{6},{7},'{8}',{9},'{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}')",
                                                uavtse.wiringNo,uavtse.meterNo,1,uavtse.customerName,uavtse.checkStatus,
                            uavtse.uavtCode,1,
                            Utilities.GetConvertedLongValue(uavtse.userSerno),
                            uavtse.districtCode,
                            Utilities.GetConvertedLongValue(uavtse.createDate),
                            uavtse.meterBrand,
                            uavtse.doorNumber,uavtse.villageCode,uavtse.streetCode,uavtse.csbmCode,
                            uavtse.indoorNumber,
                            uavtse.siteName,
                            uavtse.blockName,
                            uavtse.meterBrandCode));
                        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                        helper.Execute(sb.ToString());
                        bckWorker.ReportProgress(i);
                        i++;
                    }
                    helper.Commit();
                } catch(Exception exc) {
                    helper.Rollback();
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("Hata oluştu {0}",JsonConvert.SerializeObject(exc,Formatting.Indented));
                }

            //getting mbs data
            //TODO: get mbs data and compare with ws mbs data
            //TODO: add or update relevant data
            devam:
                i = 1;
                var mbsDt = helper.Select(String.Format("SELECT * FROM {0}",selectedItem.TableName + "_MBS"));
                var mbsItemList = new List<MBS>();
                foreach(DataRow rowItem in mbsDt.Rows) {
                    var item = new MBS();
                    item.Id = int.Parse(rowItem.ItemArray[0].ToString());
                    item.TesisatNo = rowItem.ItemArray[1].ToString();
                    item.Unvan = rowItem.ItemArray[2].ToString();
                    if(!String.IsNullOrEmpty(rowItem.ItemArray[3].ToString().Trim())) {
                        item.IlkSozlesmeTarihi = long.Parse(rowItem.ItemArray[3].ToString());
                    }
                    mbsItemList.Add(item);
                }


                var mbsList = GetMbsData();
                if(mbsList == null) {
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).WarnFormat("MBS Verisi Bulunamadı");
                    goto finish;
                }
                if(mbsList.Count < 1) {
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).WarnFormat("MBS Verisi Bulunamadı");
                    goto finish;
                }
                helper.BeginTransaction();
                try {
                    foreach(var aboneBilgi in mbsList) {
                        var tesNoStr = aboneBilgi.TESISAT_NO.ToString();
                        var unvan = String.IsNullOrEmpty(aboneBilgi.UNVAN) ? "" : aboneBilgi.UNVAN;
                        if(mbsItemList.Any(a => a.TesisatNo == tesNoStr)) {
                            //böyle bir tesisat no var
                            var sb = new StringBuilder();
                            sb.Append(
                                String.Format(
                                    "UPDATE {0}_MBS SET UNVAN='{1}',ILK_SOZLESME_TARIHI={2} WHERE TESISAT_NO='{3}'",
                                    selectedItem.TableName,
                                    unvan,
                                    aboneBilgi.SOZLESME_TARIHI.ToString().Replace(",","").Replace(".","").Substring(0,aboneBilgi.SOZLESME_TARIHI.ToString().Length - 1),
                                    tesNoStr));
                            log4net.LogManager.GetLogger(
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
                                .ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                            helper.Execute(sb.ToString());
                        } else {
                            var sb = new StringBuilder();
                            sb.Append(
                                String.Format(
                                    "INSERT INTO {0}_MBS(TESISAT_NO,UNVAN,ILK_SOZLESME_TARIHI) VALUES('{1}','{2}',{3}) ",
                                    selectedItem.TableName,tesNoStr,unvan,
                                    aboneBilgi.SOZLESME_TARIHI.ToString().Replace(",","").Replace(".","").Substring(0,aboneBilgi.SOZLESME_TARIHI.ToString().Length - 1)));
                            log4net.LogManager.GetLogger(
                                System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
                                .ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                            helper.Execute(sb.ToString());
                        }
                        bckWorker.ReportProgress(i);
                        i++;
                    }

                    helper.Commit();
                } catch(Exception exc) {
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("Hata oluştu {0}",JsonConvert.SerializeObject(exc,Formatting.Indented));
                    helper.Rollback();
                }

            finish:
                //conf a güncelleme verisi ve seçilmiş şehir bilgisini yaz
                try {

                    helper.Execute("DELETE FROM CONFIGURATION");
                    var sb = new StringBuilder();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('LAST_SYNC_DATE','{0}')",
                        Utilities.DateTimeNowLong()));
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_COUNTY_CODE','{0}')",
                        selectedItem.Code));
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_COUNTY_NAME','{0}')",
                        selectedItem.Name));
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_COUNTY_DB_CODE','{0}')",
                       selectedItem.DbCode));
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_CLASS_NAME','{0}')",
                       selectedItem.ClassName));
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());
                } catch(Exception exc) {
                    log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).ErrorFormat("Hata oluştu {0}",JsonConvert.SerializeObject(exc,Formatting.Indented));

                }


                conn.Close();
                bckWorker.ReportProgress(500000);
            }
        }

        void bckWorker_ProgressChanged(object sender,ProgressChangedEventArgs e) {
            lblCount.Text = e.ProgressPercentage.ToString();
            pbStatus.Value = e.ProgressPercentage;
            if(e.ProgressPercentage == 500000) {
                btnStart.BeginInvoke((Action)(() => {
                    btnStart.Enabled = true;
                }));
            }
        }

        private void chDate_CheckedChanged(object sender,EventArgs e) {
            if(chDate.Checked) {
                isDateSelected = true;
            }
        }

        private void btnCancel_Click(object sender,EventArgs e) {
            Application.Exit();
        }
    }

    public class District {
        public List<District> Districts = new List<District>();

        public string Code { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
        public string DbCode { get; set; }
        public string ClassName { get; set; }
        public string DbDistrictCode { get; set; }

        public District() {

        }

        public List<District> GetDistricts() {
            //1115	AKÇAKALE            	           
            //1194	BİRECİK             	          
            //1209	BOZOVA              	          
            //1220	CEYLANPINAR         	           
            //2091	EYYÜBİYE            	          
            //1378	HALFETİ             	          
            //2092	HALİLİYE            	           
            //1800	HARRAN              	            
            //1393	HİLVAN              	            
            //2093	KARAKÖPRÜ           	           
            //1630	SİVEREK             	           
            //1643	SURUÇ               	            
            //1713	VİRANŞEHİR          	  

            Districts.AddRange(new List<District>{
                    new District {Code="32",Name="EYYÜBİYE",TableName = "EYYUBIYE",DbCode = "2091",ClassName="com.rdlab.model.Eyyubiye",DbDistrictCode="1771"  },
                    new District {Code="33",Name="HALİLİYE",TableName = "HALILIYE",DbCode = "2092",ClassName="com.rdlab.model.Haliliye",DbDistrictCode="1773" },
                    new District {Code="34",Name="KARAKÖPRÜ",TableName = "KARAKOPRU",DbCode = "2093",ClassName="com.rdlab.model.Karakopru",DbDistrictCode="1775" },
                    new District {Code="35",Name="AKÇAKALE",TableName = "AKCAKALE",DbCode = "1115",ClassName="com.rdlab.model.Akcakale",DbDistrictCode="1392" },
                    new District {Code="36",Name="BİRECİK",TableName = "BIRECIK",DbCode = "1194",ClassName="com.rdlab.model.Birecik",DbDistrictCode="1394"  },
                    new District {Code="37",Name="BOZOVA",TableName = "BOZOVA",DbCode = "1209",ClassName="com.rdlab.model.Bozova",DbDistrictCode="1396"  },
                    new District {Code="38",Name="CEYLANPINAR",TableName = "CEYLANPINAR",DbCode = "1220",ClassName="com.rdlab.model.Ceylanpinar",DbDistrictCode="1399" },
                    new District {Code="39",Name="HALFETİ",TableName = "HALFETI",DbCode = "1378",ClassName="com.rdlab.model.Halfeti",DbDistrictCode="1400"  },
                    new District {Code="40",Name="HARRAN",TableName = "HARRAN",DbCode = "1800",ClassName="com.rdlab.model.Harran",DbDistrictCode="1401"},
                    new District {Code="41",Name="HİLVAN",TableName = "HILVAN",DbCode = "1393",ClassName="com.rdlab.model.Hilvan",DbDistrictCode="1402"},
                    new District {Code="42",Name="SİVEREK",TableName = "SIVEREK",DbCode = "1630",ClassName="com.rdlab.model.Siverek",DbDistrictCode="1405" },
                    new District {Code="43",Name="SURUÇ",TableName = "SURUC",DbCode = "1643",ClassName="com.rdlab.model.Suruc",DbDistrictCode="1412"},
                    new District {Code="44",Name="VİRANŞEHİR",TableName = "VIRANSEHIR",DbCode = "1713",ClassName="com.rdlab.model.Viransehir",DbDistrictCode="1414"}});

            return Districts;
        }
    }

    public class AuditLogModel {
        public long UserSerno { get; set; }
        public string AuditOptionSelection { get; set; }
        public string AuditFormSerno { get; set; }
        public string AuditFormDescription { get; set; }
        public string AuditProgressStatus { get; set; }
        public string AuditedCheckStatus { get; set; }
        public string AuditStatus { get; set; }
        public string DistrictCode { get; set; }
        public string VillageCode { get; set; }
        public string StreetCode { get; set; }
        public string CsbmCode { get; set; }
        public string DoorNumber { get; set; }
        public string SiteName { get; set; }
        public string BlockName { get; set; }
        public string IndoorNumber { get; set; }
        public string UavtCode { get; set; }
        public long CreateDate { get; set; }
        public int RecordStatus { get; set; }
    }

    public class FetchRequestModel {
        public string DistrictCode { get; set; }
        public long? LastProcessDate { get; set; }
    }

    public class Uavts {
        public decimal serno { get; set; }
        public Nullable<int> checkStatus { get; set; }
        public Nullable<decimal> createDate { get; set; }
        public string customerName { get; set; }
        public string districtCode { get; set; }
        public string doorNumber { get; set; }
        public Nullable<int> existOnUavt { get; set; }
        public string meterBrand { get; set; }
        public string meterNo { get; set; }
        public string uavtCode { get; set; }
        public Nullable<decimal> userSerno { get; set; }
        public string wiringNo { get; set; }
        public string villageCode { get; set; }
        public string streetCode { get; set; }
        public string csbmCode { get; set; }
        public string indoorNumber { get; set; }
        public Nullable<decimal> processDate { get; set; }
        public string siteName { get; set; }
        public string blockName { get; set; }
        public string meterBrandCode { get; set; }
    }

    public class DistrictValue {
        public string Id { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string VillageCode { get; set; }
        public string VillageName { get; set; }
        public string StreetCode { get; set; }
        public string StreetName { get; set; }
        public string CSBMCode { get; set; }
        public string CSBMName { get; set; }
        public string BuildingCode { get; set; }
        public string DoorNumber { get; set; }
        public string SiteName { get; set; }
        public string BlockName { get; set; }
        public string UAVTAddressNo { get; set; }
        public string IndoorNumber { get; set; }
        public string CheckStatus { get; set; }
    }

    public class ABONE_BILGI {
        public long Id { get; set; }
        public Nullable<decimal> BOLGE_KODU { get; set; }
        public string BOLGE_ADI { get; set; }
        public Nullable<decimal> KARNE_NO { get; set; }
        public string KARNE_ADRESI { get; set; }
        public Nullable<decimal> TESISAT_NO { get; set; }
        public string MARKA { get; set; }
        public string SAYAC_MARKA_ADI { get; set; }
        public Nullable<decimal> SAYAC_NO { get; set; }
        public string UNVAN { get; set; }
        public string ADRES { get; set; }
        public Nullable<decimal> SOZLESME_TARIHI { get; set; }
        public Nullable<decimal> IPTAL_TARIHI { get; set; }
        public string SOZLESME_UNVANI { get; set; }
    }

    public class MBS {
        public int Id { get; set; }
        public String TesisatNo { get; set; }
        public String Unvan { get; set; }
        public long IlkSozlesmeTarihi { get; set; }

    }
}
