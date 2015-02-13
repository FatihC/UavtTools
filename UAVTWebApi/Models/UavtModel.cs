using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAVTWebapi.Models {
    public class UavtModel {
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
        public int CheckStatus { get; set; }
    }
}