using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UAVT_Exporter.Utils {
    public static class Constants {
        public const string DISTRICT_INSERT = "INSERT INTO {0} " +
                                            "(COUNTY_CODE," +
                                            "DISTRICT_CODE," +
                                            "DISTRICT_NAME," +
                                            "VILLAGE_CODE," +
                                            "VILLAGE_NAME," +
                                            "STREET_CODE," +
                                            "STREET_NAME," +
                                            "CSBM_CODE" +
                                            ",CSBM_NAME," +
                                            "BUILDING_CODE," +
                                            "DOOR_NUMBER," +
                                            "SITE_NAME," +
                                            "BLOCK_NAME," +
                                            "UAVT_ADDRESS_NO," +
                                            "INDOOR_NUMBER," +
                                            "CHECK_STATUS) VALUES ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}',{16})";

        public const string DISTRICT_UPDATE = "UPDATE {0} SET CHECK_STATUS={1} WHERE UAVT_ADDRESS_NO='{2}'";

        public const string PUSH_REQUEST_CREATE = "INSERT INTO PUSH_REQUEST(WIRING_NO" +
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
                                                  ") VALUES ('{0}','{1}',{2},'{3}','{4}','{5}',{6},{7},'{8}',{9},'{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}')";

        public const string MBS_UPDATE = "UPDATE {0}_MBS SET UNVAN='{1}',ILK_SOZLESME_TARIHI={2} WHERE TESISAT_NO='{3}'";

        public const string MBS_INSERT = "INSERT INTO {0}_MBS(TESISAT_NO,UNVAN,ILK_SOZLESME_TARIHI) VALUES('{1}','{2}',{3}) ";

        public const string AUDIT_LOG_INSERT ="INSERT INTO AUDIT_LOG(" +
                                              "USER_SERNO," +
                                              "AUDIT_FORM_DESCRIPTION," +
                                              "AUDIT_FORM_SERNO," +
                                              "AUDIT_OPTION_SELECTION," +
                                              "AUDIT_PROGRESS_STATUS," +
                                              "AUDITED_CHECK_STATUS," +
                                              "AUDIT_STATUS," +
                                              "DISTRICT_CODE," +
                                              "VILLAGE_CODE," +
                                              "STREET_CODE," +
                                              "CSBM_CODE," +
                                              "DOOR_NUMBER," +
                                              "SITE_NAME," +
                                              "BLOCK_NAME," +
                                              "INDOOR_NUMBER," +
                                              "UAVT_CODE," +
                                              "CREATE_DATE," +
                                              "PUSHED," +
                                              "RECORD_STATUS," +
                                              "AUDIT_FORM_SERNO_TEXT) VALUES({0},'{1}','{2}','{3}','{4}','{5}'" +
                                              ",'{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}'" +
                                              ",'{14}','{15}',{16},{17},{18},{19})";
    }
}
