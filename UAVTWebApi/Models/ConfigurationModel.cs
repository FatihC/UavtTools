using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAVTWebapi.Models {
    public class ConfigurationModel {
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public string DistrictCode { get; set; }
        public string MappedDistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string VillageCode { get; set; }
        public string VillageName { get; set; }
        public string TableName{ get; set; }
        public string ClassName { get; set; }
    }
}