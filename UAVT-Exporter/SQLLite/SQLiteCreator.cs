using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using UAVT.Utils;
using UAVT_Exporter.Model;
using UAVT_Exporter.Utils;

namespace UAVT_Exporter.SQLLite {
    public class SQLiteCreator {

        private static readonly ILog _log = LogManager.GetLogger(typeof(SQLiteCreator));

        private static void CreateSqlLiteDatabase(string databaseFilePath,string sourceFile) {
            _log.Debug("Creating SQLite database...");

            if(File.Exists(databaseFilePath)) {
                _log.Debug("Deleted previous sqlite database");
                File.Delete(databaseFilePath);
            }
            _log.Debug("SQLite file was created successfully at [" + databaseFilePath + "]");
            File.Copy(sourceFile,databaseFilePath);
        }

        private static void PrepareDbFileForProcess(string databaseFilePath,List<Configuration> districts,Configuration selectedItem) {
            try {
                var connStr = "data source=" + databaseFilePath;
                using(var conn = new SQLiteConnection(connStr)) {
                    conn.Open();
                    var cmd = new SQLiteCommand(conn);
                    var helper = new SQLiteHelper(cmd);

                    _log.Debug("Connection opened for dropping non related tables");
                    foreach(var district in districts) {
                        if(selectedItem.MappedDistrictCode != district.MappedDistrictCode) {
                            var tableName = district.TableName;
                            var tableMBSName = district.TableName + "_MBS";
                            helper.DropTable(tableName);
                            helper.DropTable(tableMBSName);
                        }
                    }
                    _log.DebugFormat("Dropping processs for selected district [{0}]-[{1}] is finished",selectedItem.MappedDistrictCode,selectedItem.CountyName);
                }
            } catch(Exception exc) {
                _log.ErrorFormat("Error occured during dropping table, error message is [{0}]",exc.Message);
            }
        }

        private static List<DistrictValue> CachingDatabaseUavtList(string databaseFilePath,Configuration selectedItem) {
            try {
                var connStr = "data source=" + databaseFilePath;
                using(var conn = new SQLiteConnection(connStr)) {
                    conn.Open();
                    var cmd = new SQLiteCommand(conn);
                    var helper = new SQLiteHelper(cmd);

                    _log.Debug("Connection opened for CachingDatabaseUavtList");
                    var dt = helper.Select(String.Format("SELECT * FROM {0}",selectedItem.TableName));
                    var districtItemList = new List<DistrictValue>();
                    foreach(DataRow rowItem in dt.Rows) {
                        var item = new DistrictValue();
                        if(selectedItem.CityCode == "63") {
                            item.Id = rowItem.ItemArray[0].ToString();
                            item.CountyCode = rowItem.ItemArray[1].ToString();
                            item.DistrictCode = rowItem.ItemArray[2].ToString();
                            item.DistrictName = rowItem.ItemArray[3].ToString();
                            item.VillageCode = rowItem.ItemArray[4].ToString();
                            item.VillageName = rowItem.ItemArray[5].ToString();
                            item.StreetCode = rowItem.ItemArray[6].ToString();
                            item.StreetName = rowItem.ItemArray[7].ToString();
                            item.CSBMCode = rowItem.ItemArray[8].ToString();
                            item.CSBMName = rowItem.ItemArray[9].ToString();
                            item.BuildingCode = rowItem.ItemArray[10].ToString();
                            item.DoorNumber = rowItem.ItemArray[11].ToString();
                            item.SiteName = rowItem.ItemArray[12].ToString();
                            item.BlockName = rowItem.ItemArray[13].ToString();
                            item.UAVTAddressNo = rowItem.ItemArray[14].ToString();
                            item.IndoorNumber = rowItem.ItemArray[15].ToString();
                            item.CheckStatus = rowItem.ItemArray[16].ToString();
                        } else {

                            item.CountyCode = rowItem.ItemArray[0].ToString();
                            item.DistrictCode = rowItem.ItemArray[1].ToString();
                            item.DistrictName = rowItem.ItemArray[2].ToString();
                            item.VillageCode = rowItem.ItemArray[3].ToString();
                            item.VillageName = rowItem.ItemArray[4].ToString();
                            item.StreetCode = rowItem.ItemArray[5].ToString();
                            item.StreetName = rowItem.ItemArray[6].ToString();
                            item.CSBMCode = rowItem.ItemArray[7].ToString();
                            item.CSBMName = rowItem.ItemArray[8].ToString();
                            item.BuildingCode = rowItem.ItemArray[9].ToString();
                            item.DoorNumber = rowItem.ItemArray[10].ToString();
                            item.SiteName = rowItem.ItemArray[11].ToString();
                            item.BlockName = rowItem.ItemArray[12].ToString();
                            item.UAVTAddressNo = rowItem.ItemArray[13].ToString();
                            item.IndoorNumber = rowItem.ItemArray[14].ToString();
                            item.CheckStatus = rowItem.ItemArray[15].ToString();
                            item.Id = rowItem.ItemArray[16].ToString();
                        }


                        districtItemList.Add(item);
                    }
                    _log.DebugFormat("CachingDatabaseUavtList processs for selected district [{0}]-[{1}] is finished",selectedItem.MappedDistrictCode,selectedItem.CountyName);
                    return districtItemList;
                }
            } catch(Exception exc) {
                _log.ErrorFormat("Error occured during CachingDatabaseUavtList, error message is [{0}]",exc.Message);
                return null;
            }

        }

        private static List<MBS> CachingDatabaseMBSList(string databaseFilePath,Configuration selectedItem) {
            try {
                var connStr = "data source=" + databaseFilePath;
                using(var conn = new SQLiteConnection(connStr)) {
                    conn.Open();
                    var cmd = new SQLiteCommand(conn);
                    var helper = new SQLiteHelper(cmd);

                    _log.Debug("Connection opened for CachingDatabaseMBSList");
                    var mbsDt = helper.Select(String.Format("SELECT * FROM {0}",selectedItem.TableName + "_MBS"));
                    var mbsItemList = new List<MBS>();
                    foreach(DataRow rowItem in mbsDt.Rows) {
                        var item = new MBS {
                            Id = int.Parse(rowItem.ItemArray[0].ToString()),
                            TesisatNo = rowItem.ItemArray[1].ToString(),
                            Unvan = rowItem.ItemArray[2].ToString()
                        };
                        if(!String.IsNullOrEmpty(rowItem.ItemArray[3].ToString().Trim())) {
                            item.IlkSozlesmeTarihi = long.Parse(rowItem.ItemArray[3].ToString());
                        }
                        mbsItemList.Add(item);
                    }
                    _log.DebugFormat("CachingDatabaseMBSList processs for selected district [{0}]-[{1}] is finished",selectedItem.MappedDistrictCode,selectedItem.CountyName);
                    return mbsItemList;
                }
            } catch(Exception exc) {
                _log.ErrorFormat("Error occured during CachingDatabaseMBSList, error message is [{0}]",exc.Message);
                return null;
            }

        }

        private static List<AuditLogModel> CachingDatabaseAuditLogList(string databaseFilePath) {
            try {
                var connStr = "data source=" + databaseFilePath;
                using(var conn = new SQLiteConnection(connStr)) {
                    conn.Open();
                    var cmd = new SQLiteCommand(conn);
                    var helper = new SQLiteHelper(cmd);

                    _log.Debug("Connection opened for CachingDatabaseAuditLogList");
                    var mbsDt = helper.Select("SELECT * FROM  AUDIT_LOG");
                    var logItemList = new List<AuditLogModel>();
                    foreach(DataRow rowItem in mbsDt.Rows) {
                        var item = new AuditLogModel {
                            UserSerno = int.Parse(rowItem.ItemArray[1].ToString()),
                            AuditFormDescription = rowItem.ItemArray[2].ToString(),
                            AuditFormSerno = rowItem.ItemArray[3].ToString(),
                            AuditOptionSelection = rowItem.ItemArray[4].ToString(),
                            AuditProgressStatus = rowItem.ItemArray[5].ToString(),
                            AuditedCheckStatus = rowItem.ItemArray[6].ToString(),
                            AuditStatus = rowItem.ItemArray[7].ToString(),
                            DistrictCode = rowItem.ItemArray[8].ToString(),
                            VillageCode = rowItem.ItemArray[9].ToString(),
                            StreetCode = rowItem.ItemArray[10].ToString(),
                            CsbmCode = rowItem.ItemArray[11].ToString(),
                            DoorNumber = rowItem.ItemArray[12].ToString(),
                            SiteName = rowItem.ItemArray[13].ToString(),
                            BlockName = rowItem.ItemArray[14].ToString(),
                            IndoorNumber = rowItem.ItemArray[15].ToString(),
                            UavtCode = rowItem.ItemArray[16].ToString(),
                            CreateDate = long.Parse(rowItem.ItemArray[17].ToString()),
                            RecordStatus = int.Parse(rowItem.ItemArray[19].ToString()),
                            AuditFormSernoText = rowItem.ItemArray[20].ToString()
                        };
                        logItemList.Add(item);
                    }
                    _log.DebugFormat("CachingDatabaseAuditLogList processs is finished");
                    return logItemList;
                }
            } catch(Exception exc) {
                _log.ErrorFormat("Error occured during CachingDatabaseAuditLogList, error message is [{0}]",exc.Message);
                return null;
            }

        }

        private static void CreateNewUavtValues(string databaseFilePath,Configuration selectedItem,List<DistrictValue> newUavtValues,List<DistrictValue> cachedValues) {
            try {
                var connStr = "data source=" + databaseFilePath;
                using(var conn = new SQLiteConnection(connStr)) {
                    conn.Open();
                    var cmd = new SQLiteCommand(conn);
                    var helper = new SQLiteHelper(cmd);

                    _log.Debug("Connection opened for CreateNewUavtValues");
                    foreach(var districtValue in newUavtValues) {
                        if(cachedValues.Any(a => a.UAVTAddressNo == districtValue.UAVTAddressNo)) {
                            _log.WarnFormat("There is specified item in the list with uavt cod[{0}]",districtValue.UAVTAddressNo);
                            continue;
                        }

                        var sql = String.Format(
                            Constants.DISTRICT_INSERT,
                            selectedItem.TableName,
                            districtValue.CountyCode,
                            districtValue.DistrictCode,
                            districtValue.DistrictName,
                            districtValue.VillageCode,
                            districtValue.VillageName,
                            districtValue.StreetCode,
                            districtValue.StreetName,
                            districtValue.CSBMCode,
                            districtValue.CSBMName,
                            districtValue.BuildingCode,
                            districtValue.DoorNumber,
                            districtValue.SiteName.NormalizeForInsert(),
                            districtValue.BlockName.NormalizeForInsert(),
                            districtValue.UAVTAddressNo,
                            districtValue.IndoorNumber,
                            districtValue.CheckStatus
                            );
                        _log.ErrorFormat("SQL query for CreateNewUavtValues  {0}",sql);
                        helper.Execute(sql);
                    }
                    _log.DebugFormat("CreateNewUavtValues processs for selected district [{0}]-[{1}] is finished",selectedItem.MappedDistrictCode,selectedItem.CountyName);
                }
            } catch(Exception exc) {
                _log.ErrorFormat("Error occured during CreateNewUavtValues, error message is [{0}]",exc.Message);
            }

        }

        private static void InsertOrUpdateUavtValues(string databaseFilePath,Configuration selectedItem,List<Uavts> uavtList,List<DistrictValue> cachedValues,SqlConversionHandler handler) {
            try {
                var connStr = "data source=" + databaseFilePath;
                using(var conn = new SQLiteConnection(connStr)) {
                    conn.Open();
                    _log.Debug("Connection opened for InsertOrUpdateUavtValues");

                    var transaction = conn.BeginTransaction();
                    try {
                        int i = 0;
                        foreach(var uavtse in uavtList) {
                            var cmd = new SQLiteCommand(conn);
                            cmd.Transaction = transaction;
                            if(cachedValues.Any(a => a.UAVTAddressNo == uavtse.uavtCode)) {

                                var sql = String.Format(Constants.DISTRICT_UPDATE,selectedItem.TableName,"7",uavtse.uavtCode);
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                                _log.DebugFormat("There is specified item in the list with uavt code[{0}]  and SQL query for  InsertOrUpdateUavtValues {1}",uavtse.uavtCode,sql);
                            } else {
                                var itemForVal =
                                    cachedValues.FirstOrDefault(
                                        a => a.CSBMCode == uavtse.csbmCode && a.StreetCode == uavtse.streetCode);
                                if(itemForVal == null) {
                                    _log.WarnFormat("There is no such an item with csbmCode {0} and streetCode {1}",uavtse.csbmCode,uavtse.streetCode);
                                    continue;
                                }
                                var sql = String.Format(
                                    Constants.DISTRICT_INSERT,
                                    selectedItem.TableName,
                                    selectedItem.CountyCode,
                                    selectedItem.DistrictCode,
                                    itemForVal.DistrictName,
                                    uavtse.villageCode,
                                    itemForVal.VillageName,
                                    uavtse.streetCode,
                                    itemForVal.StreetName,
                                    uavtse.csbmCode,
                                    itemForVal.CSBMName,
                                    itemForVal.BuildingCode,
                                    uavtse.doorNumber,
                                    itemForVal.SiteName.NormalizeForInsert(),
                                    itemForVal.BlockName.NormalizeForInsert(),
                                    uavtse.uavtCode,
                                    uavtse.indoorNumber,
                                    "7"
                                    );
                                _log.ErrorFormat("SQL query for  InsertOrUpdateUavtValues {0}",sql);
                                cmd.CommandText = sql;
                                cmd.ExecuteNonQuery();
                            }
                            i++;
                            handler(false,true,i,"Uavt güncelleme");
                        }
                        transaction.Commit();
                        _log.DebugFormat("InsertOrUpdateUavtValues processs for selected district [{0}]-[{1}] is finished",selectedItem.MappedDistrictCode,selectedItem.CountyName);
                    } catch(Exception exc) {
                        transaction.Rollback();
                        _log.ErrorFormat("Error occured during InsertOrUpdateUavtValues, error message is [{0}]",exc.Message);
                    }
                }
            } catch(Exception exc) {
                _log.ErrorFormat("Error occured during InsertOrUpdateUavtValues, error message is [{0}]",exc.Message);
            }
        }

        private static void InsertingDataToPushRequestTable(string databaseFilePath,Configuration selectedItem,List<Uavts> uavtList,List<DistrictValue> cachedValues,SqlConversionHandler handler) {
            try {
                var connStr = "data source=" + databaseFilePath;
                using(var conn = new SQLiteConnection(connStr)) {
                    conn.Open();
                    _log.Debug("Connection opened for InsertingDataToPushRequestTable");

                    var transaction = conn.BeginTransaction();
                    try {
                        int i = 0;
                        var cmd = new SQLiteCommand(conn);
                        var helper = new SQLiteHelper(cmd);
                        foreach(var uavtse in uavtList) {
                            var sql = String.Format(Constants.PUSH_REQUEST_CREATE,
                                uavtse.wiringNo,uavtse.meterNo,1,uavtse.customerName,uavtse.checkStatus,
                                uavtse.uavtCode,1,
                                Utilities.GetConvertedLongValue(uavtse.userSerno),
                                uavtse.districtCode,
                                Utilities.GetConvertedLongValue(uavtse.createDate),
                                uavtse.meterBrand,
                                uavtse.doorNumber,uavtse.villageCode,uavtse.streetCode,uavtse.csbmCode,
                                uavtse.indoorNumber,
                                uavtse.siteName.NormalizeForInsert(),
                                uavtse.blockName.NormalizeForInsert(),
                                uavtse.meterBrandCode);
                            _log.DebugFormat("SQL Sorgusu {0}",sql);
                            helper.Execute(sql);

                            i++;
                            handler(false,true,i,"Ara tablo güncelleme");
                        }
                        transaction.Commit();
                        _log.DebugFormat("InsertingDataToPushRequestTable processs for selected district [{0}]-[{1}] is finished",selectedItem.MappedDistrictCode,selectedItem.CountyName);
                    } catch(Exception exc) {
                        transaction.Rollback();
                        _log.ErrorFormat("Error occured during InsertingDataToPushRequestTable, error message is [{0}]",exc.Message);
                    }
                }
            } catch(Exception exc) {
                _log.ErrorFormat("Error occured during InsertingDataToPushRequestTable, error message is [{0}]",exc.Message);
            }
        }

        private static void InsertOrUpdateMBSValues(string databaseFilePath,Configuration selectedItem,List<SubscriberModel> mbsList,List<MBS> cachedValues,SqlConversionHandler handler) {
            try {
                var connStr = "data source=" + databaseFilePath;
                using(var conn = new SQLiteConnection(connStr)) {
                    conn.Open();
                    _log.Debug("Connection opened for InsertingDataToPushRequestTable");

                    var transaction = conn.BeginTransaction();
                    try {
                        int i = 0;
                        var cmd = new SQLiteCommand(conn);
                        var helper = new SQLiteHelper(cmd);
                        foreach(var aboneBilgi in mbsList) {
                            var tesNoStr = aboneBilgi.TesisatNo;
                            var unvan = String.IsNullOrEmpty(aboneBilgi.Unvan) ? "" : aboneBilgi.Unvan;
                            if(cachedValues.Any(a => a.TesisatNo == tesNoStr)) {
                                //böyle bir tesisat no var
                                var sql = String.Format(
                                    Constants.MBS_UPDATE,
                                    selectedItem.TableName,
                                    unvan,
                                    aboneBilgi.SozlesmeTarihi
                                        .Replace(",","")
                                        .Replace(".","")
                                        .Substring(0,aboneBilgi.SozlesmeTarihi.Length - 1),
                                    tesNoStr);
                                _log.ErrorFormat("SQL Sorgusu {0}",sql);
                                helper.Execute(sql);
                            } else {
                                var sql = String.Format(
                                    Constants.MBS_INSERT,
                                    selectedItem.TableName,tesNoStr,unvan,
                                    aboneBilgi.SozlesmeTarihi
                                        .Replace(",","")
                                        .Replace(".","")
                                        .Substring(0,aboneBilgi.SozlesmeTarihi.Length - 1));
                                _log.ErrorFormat("SQL Sorgusu {0}",sql);
                                helper.Execute(sql);
                            }
                            handler(false,true,i,"MBS tablo güncelleme");
                            i++;
                        }
                        transaction.Commit();
                        _log.DebugFormat("InsertOrUpdateMBSValues processs for selected district [{0}]-[{1}] is finished",selectedItem.MappedDistrictCode,selectedItem.CountyName);
                    } catch(Exception exc) {
                        transaction.Rollback();
                        _log.ErrorFormat("Error occured during InsertOrUpdateMBSValues, error message is [{0}]",exc.Message);
                    }
                }
            } catch(Exception exc) {
                _log.ErrorFormat("Error occured during InsertOrUpdateMBSValues, error message is [{0}]",exc.Message);
            }
        }

        private static void InsertOrUpdateLogsValue(string databaseFilePath,List<AuditLogModel> logList,List<AuditLogModel> cachedValues,SqlConversionHandler handler) {
            try {
                var connStr = "data source=" + databaseFilePath;
                using(var conn = new SQLiteConnection(connStr)) {
                    conn.Open();
                    _log.Debug("Connection opened for InsertOrUpdateLogsValue");

                    var transaction = conn.BeginTransaction();
                    try {
                        int i = 0;
                        var cmd = new SQLiteCommand(conn);
                        var helper = new SQLiteHelper(cmd);
                        foreach(var auditLog in logList) {
                            var sql = String.Format(
                                    Constants.AUDIT_LOG_INSERT,
                                    auditLog.UserSerno,
                                    auditLog.AuditFormDescription.NormalizeForInsert(),
                                    auditLog.AuditFormSerno,
                                    auditLog.AuditOptionSelection,
                                    auditLog.AuditProgressStatus,
                                    auditLog.AuditedCheckStatus,
                                    auditLog.AuditStatus,
                                    auditLog.DistrictCode,
                                    auditLog.VillageCode,
                                    auditLog.StreetCode,
                                    auditLog.CsbmCode,
                                    auditLog.DoorNumber,
                                    auditLog.SiteName.NormalizeForInsert(),
                                    auditLog.BlockName.NormalizeForInsert(),
                                    auditLog.IndoorNumber,
                                    auditLog.UavtCode,
                                    auditLog.CreateDate,
                                    true,
                                    auditLog.RecordStatus,
                                    auditLog.AuditFormSernoText);
                            _log.ErrorFormat("SQL Sorgusu {0}",sql);
                            helper.Execute(sql);
                            handler(false,true,i,"Tespit tablo güncelleme");
                            i++;
                        }
                        transaction.Commit();
                        _log.DebugFormat("InsertOrUpdateLogsValue processs is finished");
                    } catch(Exception exc) {
                        transaction.Rollback();
                        _log.ErrorFormat("Error occured during InsertOrUpdateLogsValue, error message is [{0}]",exc.Message);
                    }
                }
            } catch(Exception exc) {
                _log.ErrorFormat("Error occured during InsertOrUpdateLogsValue, error message is [{0}]",exc.Message);
            }
        }

        private static void UpdateConfigurationTable(string databaseFilePath,Configuration selectedItem) {
            try {
                var connStr = "data source=" + databaseFilePath;
                using(var conn = new SQLiteConnection(connStr)) {
                    conn.Open();
                    var cmd = new SQLiteCommand(conn);
                    var helper = new SQLiteHelper(cmd);

                    _log.Debug("Connection opened for UpdateConfigurationTable");
                    helper.Execute("DELETE FROM CONFIGURATION");
                    var sb = new StringBuilder();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('LAST_SYNC_DATE','{0}')",
                        Utilities.DateTimeNowLong()));
                    _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('LAST_PUSH_DATE','{0}')",
                        Utilities.DateTimeNowLong()));
                    _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_COUNTY_CODE','{0}')",
                        selectedItem.MappedDistrictCode));
                    _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_COUNTY_NAME','{0}')",
                        selectedItem.CountyName));
                    _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_COUNTY_DB_CODE','{0}')",
                       selectedItem.CountyCode));
                    _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_CLASS_NAME','{0}')",
                       selectedItem.ClassName));
                    _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_CITY_CODE','{0}')",
                       selectedItem.CityCode));
                    _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_CITY_NAME','{0}')",
                       selectedItem.CityName));
                    _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_DISTRICT_CODE','{0}')",
                       selectedItem.DistrictCode));
                    _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_DISTRICT_NAME','{0}')",
                       selectedItem.DistrictName));
                    _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_VILLAGE_CODE','{0}')",
                       selectedItem.VillageCode));
                    _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    sb.Clear();
                    sb.Append(String.Format("INSERT INTO CONFIGURATION(KEY,VALUE) VALUES ('SELECTED_VILLAGE_NAME','{0}')",
                       selectedItem.VillageName));
                    _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                    helper.Execute(sb.ToString());

                    _log.DebugFormat("UpdateConfigurationTable processs for selected district [{0}]-[{1}] is finished",selectedItem.MappedDistrictCode,selectedItem.DistrictCode);
                }
            } catch(Exception exc) {
                _log.ErrorFormat("Error occured during UpdateConfigurationTable, error message is [{0}]",exc.Message);
            }

        }

        private static void UpdateUsers(string databaseFilePath,List<Users> users,SqlConversionHandler handler) {
            try {
                var i = 0;
                var connStr = "data source=" + databaseFilePath;
                using(var conn = new SQLiteConnection(connStr)) {
                    conn.Open();
                    var cmd = new SQLiteCommand(conn);
                    var helper = new SQLiteHelper(cmd);

                    _log.Debug("Connection opened for UpdateUsers");
                    helper.Execute("DELETE FROM USERS");
                    handler(false,true,i,"Kullanıcı Güncelleme");
                    foreach(var userse in users) {
                        var sb = new StringBuilder();
                        sb.Append(String.Format("INSERT INTO USERS(USER_SERNO,USERNAME,PASSWORD,FULL_NAME,STATUS,CREATE_DATE) " +
                                                "VALUES ({0},'{1}','{2}','{3}',{4},{5})",
                            userse.userSerno,
                            userse.username,
                            userse.password,
                            userse.fullName,
                            userse.status,
                            Utilities.DateTimeNowLong()));
                        _log.ErrorFormat("SQL Sorgusu {0}",sb.ToString());
                        helper.Execute(sb.ToString());
                        handler(false,true,i,"Kullanıcı Güncelleme");
                        i++;
                    }

                    handler(false,true,i,"Kullanıcı Güncelleme Tamamlandı.");
                    _log.DebugFormat("UpdateUsers processs for selected district ");
                }
            } catch(Exception exc) {
                _log.ErrorFormat("Error occured during UpdateUsers, error message is [{0}]",exc.Message);
            }

        }

        public static void CreateSqliteDb(string sqlitePath,
            string sourceFile,
            List<Configuration> districtList,
            Configuration selectedItem,
            List<DistrictValue> newUavtValues,
            List<Uavts> uavtList,
            List<SubscriberModel> mbsValues,
            List<AuditLogModel> auditLogs,
            List<Users> users,
            SqlConversionHandler handler) {
            WaitCallback wc = (delegate(object state) {
                try {
                    CreateSqliteDbFile(sqlitePath,sourceFile,districtList,selectedItem,newUavtValues,uavtList,mbsValues,auditLogs,users,handler);
                    handler(true,true,500000,"İşlem tamamlandı");
                } catch(Exception ex) {
                    _log.Error("Failed to convert SQL Server database to SQLite database",ex);
                    handler(true,false,100,ex.Message);
                }
            });
            ThreadPool.QueueUserWorkItem(wc);
        }

        private static void CreateSqliteDbFile(string sqlitePath,
            string sourceFile,
            List<Configuration> districtList,
            Configuration selectedItem,
            List<DistrictValue> newUavtValues,
            List<Uavts> uavtList,
            List<SubscriberModel> mbsValues,
            List<AuditLogModel> auditLogs,
            List<Users> users,
            SqlConversionHandler handler) {

            //moving original file with different name
            CreateSqlLiteDatabase(sqlitePath,sourceFile);

            //dropping non related tables in sql file
            PrepareDbFileForProcess(sqlitePath,districtList,selectedItem);

            //gathering uavt list from predefined table
            var dbUavtList = CachingDatabaseUavtList(sqlitePath,selectedItem);

            if(newUavtValues != null && newUavtValues.Count > 0) {
                //copy new uavts
                CreateNewUavtValues(sqlitePath,selectedItem,newUavtValues,dbUavtList);
            }

            if(uavtList != null && uavtList.Count > 0) {
                //inserting or updating uavt values
                InsertOrUpdateUavtValues(sqlitePath,selectedItem,uavtList,dbUavtList,handler);

                //inserting relative datas to push request
                InsertingDataToPushRequestTable(sqlitePath,selectedItem,uavtList,dbUavtList,handler);
            }

            if(mbsValues != null && mbsValues.Count > 0) {
                var mbsList = CachingDatabaseMBSList(sqlitePath,selectedItem);
                InsertOrUpdateMBSValues(sqlitePath,selectedItem,mbsValues,mbsList,handler);
            }

            if(auditLogs != null && auditLogs.Count > 0) {
                var cachedAuditList = CachingDatabaseAuditLogList(sqlitePath);
                InsertOrUpdateLogsValue(sqlitePath,auditLogs,cachedAuditList,handler);
            }

            if(users != null && users.Count > 0) {
                UpdateUsers(sqlitePath,users,handler);
            }

            UpdateConfigurationTable(sqlitePath,selectedItem);

        }
    }

    public delegate void SqlConversionHandler(bool done,bool success,int percent,string msg);
}
