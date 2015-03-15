using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAVTWebapi.Models {
    public class AuditLogModel {
        public long UserSerno { get; set; }
        public string AuditOptionSelection { get; set; }
        public string AuditFormSerno { get; set; }
        public string AuditFormSernoText { get; set; }
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
}