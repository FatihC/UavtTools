//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UAVTWebapi.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class AuditLogs
    {
        public decimal Serno { get; set; }
        public Nullable<decimal> UserSerno { get; set; }
        public string OptionSelection { get; set; }
        public string FormSerno { get; set; }
        public string FormDescription { get; set; }
        public string ProgressStatus { get; set; }
        public string DistrictCode { get; set; }
        public string VillageCode { get; set; }
        public string StreetCode { get; set; }
        public string CsbmCode { get; set; }
        public string DoorNumber { get; set; }
        public string SiteName { get; set; }
        public string BlockName { get; set; }
        public string IndoorNumber { get; set; }
        public string UavtCode { get; set; }
        public Nullable<decimal> CreateDate { get; set; }
        public Nullable<decimal> ProcessDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string PreviousCheckStatus { get; set; }
        public string Status { get; set; }
    }
}