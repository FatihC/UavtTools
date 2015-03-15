using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAVT_Exporter.Model {
    public class Configuration {
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string CountyCode { get; set; }
        public string CountyName { get; set; }
        public string DistrictCode { get; set; }
        public string MappedDistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string VillageCode { get; set; }
        public string VillageName { get; set; }
        public string TableName { get; set; }
        public string ClassName { get; set; }
    }

    public  class Users {
        public decimal id { get; set; }
        public Nullable<decimal> createDate { get; set; }
        public string fullName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<double> userSerno { get; set; }
    }

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

    public class SubscriberModel {
        public long Id;
        public string BolgeKodu;
        public string BolgeAdi;
        public string KarneNo;
        public string KarneAdresi;
        public string TesisatNo;
        public string Marka;
        public string SayacMarkaAdi;
        public string SayacNo;
        public string Unvan;
        public string Adres;
        public string SozlesmeTarihi;
        public string IptalTarihi;
        public string SozlesmeUnvani;
    }

    public class MBS {
        public int Id { get; set; }
        public String TesisatNo { get; set; }
        public String Unvan { get; set; }
        public long IlkSozlesmeTarihi { get; set; }

    }

    public class City {
        public string CityCode { get; set; }
        public string CityName { get; set; }

        public static List<City> GetCities() {
            return new List<City>()
            {
                new City{CityCode = "21",CityName = "Diyarbakır"},
                new City{CityCode = "47",CityName = "Mardin"},
                new City{CityCode = "56",CityName = "Siirt"},
                new City{CityCode = "63",CityName = "Urfa"},
                new City{CityCode = "72",CityName = "Batman"},
                new City{CityCode = "73",CityName = "Şırnak"}
            };
        }
    }
}
