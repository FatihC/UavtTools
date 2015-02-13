using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAVTWebapi.Models {
    public class UavtDataModel {
        public decimal Serno { get; set; }
        public Nullable<int> CheckStatus { get; set; }
        public Nullable<decimal> CreateDate { get; set; }
        public string CustomerName { get; set; }
        public string DistrictCode { get; set; }
        public string DoorNumber { get; set; }
        public Nullable<int> ExistOnUavt { get; set; }
        public string MeterBrand { get; set; }
        public string MeterBrandCode { get; set; }
        public string SiteName { get; set; }
        public string BlockName { get; set; }
        public string MeterNo { get; set; }
        public string UavtCode { get; set; }
        public Nullable<decimal> UserSerno { get; set; }
        public string WiringNo { get; set; }
        public string VillageCode { get; set; }
        public string StreetCode { get; set; }
        public string CsbmCode { get; set; }
        public string IndoorNumber { get; set; }
    }
}