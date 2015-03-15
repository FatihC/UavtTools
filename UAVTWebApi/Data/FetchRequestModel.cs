using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UAVTWebapi.Models {
    public class FetchRequestModel {
        public string DistrictCode { get; set; }
        public long? LastProcessDate { get; set; }
    }
}