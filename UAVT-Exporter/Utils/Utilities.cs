using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Windows.Forms;
using UAVT_Exporter;

namespace UAVT.Utils {
    public static class Utilities {
        public static string YYYY = "yyyy";
        public static string MM = "MM";
        public static string DD = "dd";
        public static string HH = "HH";
        public static string mm = "mm";
        public static string SS = "ss";

        public static string yyyyMMdd = YYYY + MM + DD;
        public static string yyyyMMddHHmm = yyyyMMdd + HH + mm;
        public static string yyyyMMddHHmmss = yyyyMMddHHmm + SS;

        public static long DateTimeNowLong() {
            long val = default(long);
            if(long.TryParse(DateTime.Now.ToString("yyyyMMddHHmmss"),out val)) { }

            return val;

        }

        public static long DateTimeNowLong(string format) {
            long val = default(long);
            if(long.TryParse(DateTime.Now.ToString(format),out val)) { }

            return val;

        }

        public static long ConvertToLong(this DateTime dt) {
            long val = default(long);
            if(long.TryParse(DateTime.Now.ToString("yyyyMMddHHmmss"),out val)) { }

            return val;

        }

        public static bool CheckWebServiceExist(string url) {
            try {
                var httpReq = WebRequest.Create(url);
                using(var resp = httpReq.GetResponse() as HttpWebResponse) {
                    if(resp.StatusCode == HttpStatusCode.OK) {
                        return true;
                    }
                    return false;
                }
            } catch(Exception ex) {
                return false;
            }
        }

        public static string CreateOrUseExistedFolder(string countyName,string path) {
            //var execPath = Application.StartupPath;
            var execPath = path;
            execPath += "\\";


            var dateNow = DateTimeNowLong(yyyyMMdd);
            execPath += dateNow;
            if(!Directory.Exists(execPath)) {
                Directory.CreateDirectory(execPath);
            }

            execPath += "\\" + countyName;
            if(!Directory.Exists(execPath)) {
                Directory.CreateDirectory(execPath);
            }

            execPath += "\\db.dat";

            return execPath;
        }

        public static string GetConvertedLongValue(decimal? value) {
            if(value == null) {
                return "";
            }

            var strVal = value.ToString().Replace(",","").Replace(".","");
            if(strVal.Length > 1) {
                strVal = strVal.Substring(0,strVal.Length - 1);
            }

            return strVal;
        }

        public static List<District> GetDistricts()
        {
            var dist = new District();
            return dist.GetDistricts();
        }

    }
}