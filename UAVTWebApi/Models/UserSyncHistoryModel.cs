using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAVTWebapi.Models {
    public class UserSyncHistoryModel {
        public decimal Serno { get; set; }
        public decimal? DistrictCode { get; set; }
        public decimal? LastMbsSyncDate { get; set; }
        public decimal? LastUavtSyncDate { get; set; }
        public decimal? UserSerno { get; set; }
    }
}