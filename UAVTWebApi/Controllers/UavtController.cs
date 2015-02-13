using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Routing.Constraints;
using Newtonsoft.Json;
using UAVTWebapi.Data;
using UAVTWebapi.Filters;
using UAVTWebapi.Models;
using UAVTWebapi.Utils;

namespace UAVTWebapi.Controllers {
    [RoutePrefix("api/v1/uavt")]
    public class UavtController : ApiController {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        readonly MediaTypeFormatter _jsonFormatter = new JsonMediaTypeFormatter {
            SerializerSettings = {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            }
        };

        [Route("IsAlive")]
        [HttpGet]
        public HttpResponseMessage IsAlive() {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            return resp;

        }

        [Route("login")]
        [HttpPost]
        public HttpResponseMessage Login(LoginModel user) {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            Log.InfoFormat("Login request come [{0}]",JsonConvert.SerializeObject(user,Formatting.Indented));

            try {
                using(var model = new Entities()) {
                    var selectedUser =
                        model.Users.FirstOrDefault(a => a.username == user.Username && a.password == user.Password);

                    if(selectedUser == null) {
                        Log.WarnFormat("User not found");
                        resp.StatusCode = HttpStatusCode.NotFound;
                        return resp;
                    }

                    Log.InfoFormat("User found [{0}]",selectedUser.userSerno);
                    Debug.Assert(selectedUser.userSerno != null,"selectedUser.userSerno != null");
                    user.Serno = (long)selectedUser.userSerno;
                    user.Username = selectedUser.fullName;
                    user.Password = "";
                    resp.Content = new ObjectContent(typeof(LoginModel),user,_jsonFormatter);
                }
            } catch(Exception exc) {
                resp.StatusCode = HttpStatusCode.InternalServerError;
                Log.InfoFormat("Error occured and exception is [{0}]",JsonConvert.SerializeObject(exc,Formatting.Indented));
            }

            return resp;

        }

        [Route("push")]
        [HttpPost]
        public HttpResponseMessage Push(dynamic uavtModelList) {

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            Log.InfoFormat("Push request come ");

            try {
                using(var model = new Entities()) {
                    if(uavtModelList.GetType() == typeof(List<string>)) {
                        Log.InfoFormat("Push request for list");
                        foreach(var uavtModelItem in uavtModelList) {
                            CreateOrUpdateUavt(uavtModelItem,model);
                            var res = model.SaveChanges();
                            Log.InfoFormat("Push request for item saved and result [{0}]",res);
                        }
                    } else {
                        CreateOrUpdateUavt(uavtModelList,model);
                        var res = model.SaveChanges();
                        Log.InfoFormat("Push request for item saved and result [{0}]",res);
                    }
                }
            } catch(Exception exc) {
                resp.StatusCode = HttpStatusCode.InternalServerError;
                Log.ErrorFormat("Error occured and exception is [{0}]",JsonConvert.SerializeObject(exc,Formatting.Indented));
            }

            return resp;
        }

        [Route("pushLogs")]
        [HttpPost]
        public HttpResponseMessage PushLogs(dynamic logModelList) {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            Log.InfoFormat("Audit log request come");
            try {
                using(var model = new Entities()) {
                    if(logModelList.GetType() == typeof(List<string>)) {
                        Log.InfoFormat("Log for list");
                        foreach(var logItem in logModelList) {
                            CreateOrUpdateLogs(logItem,model);
                            var res = model.SaveChanges();
                            Log.InfoFormat("Audit log for item saved and result [{0}]",res);
                        }
                    } else {
                        CreateOrUpdateLogs(logModelList,model);
                        var res = model.SaveChanges();
                        Log.InfoFormat("Audit log for item saved and result [{0}]",res);
                    }
                }
            } catch(Exception exc) {
                resp.StatusCode = HttpStatusCode.InternalServerError;
                Log.ErrorFormat("Error occured and exception is [{0}]",JsonConvert.SerializeObject(exc,Formatting.Indented));
            }
            return resp;

        }

        [Deflate]
        [Route("fetch")]
        [HttpPost]
        public HttpResponseMessage Fetch(FetchRequestModel reqModel) {
            var resp = new HttpResponseMessage();
            try {
                Log.InfoFormat("Fetch request come [{0}]",JsonConvert.SerializeObject(reqModel,Formatting.Indented));
                using(var model = new Entities()) {
                    //create date max olanı al
                    long now = Utilities.DateTimeNowLong();
                    if(reqModel.LastProcessDate == null) {
                        reqModel.LastProcessDate = 20121010000000;
                    }

                    var uavts =
                        model.Uavts.Where(
                            a => a.processDate >= reqModel.LastProcessDate && a.processDate <= now && a.districtCode == reqModel.DistrictCode)
                            .ToList();
                    var mergedVal = uavts.OrderByDescending(x => x.createDate)
                        .GroupBy(
                            x =>
                                new {
                                    x.csbmCode,
                                    x.districtCode,
                                    x.doorNumber,
                                    x.indoorNumber,
                                    x.streetCode,
                                    x.villageCode
                                }).Select(a => new { a,count = a.Count() }).SelectMany(b => b.a.Select(c => c).Zip(Enumerable.Range(1,b.count),
                            (j,i) =>
                                new {
                                    j.csbmCode,
                                    j.createDate,
                                    j.checkStatus,
                                    j.customerName,
                                    j.districtCode,
                                    j.doorNumber,
                                    j.existOnUavt,
                                    j.indoorNumber,
                                    j.meterBrand,
                                    j.meterNo,
                                    j.processDate,
                                    j.streetCode,
                                    j.uavtCode,
                                    j.userSerno,
                                    j.villageCode,
                                    j.wiringNo,
                                    j.blockName,
                                    j.siteName,
                                    j.meterBrandCode,
                                    rn = i
                                }));
                    var uavtList = (from item in mergedVal
                                    where item.rn == 1
                                    select new Uavts() {
                                        csbmCode = item.csbmCode,
                                        createDate = item.createDate,
                                        checkStatus = item.checkStatus,
                                        customerName = item.customerName,
                                        districtCode = item.districtCode,
                                        doorNumber = item.doorNumber,
                                        existOnUavt = item.existOnUavt,
                                        indoorNumber = item.indoorNumber,
                                        meterBrand = item.meterBrand,
                                        meterNo = item.meterNo,
                                        processDate = item.processDate,
                                        streetCode = item.streetCode,
                                        uavtCode = item.uavtCode,
                                        userSerno = item.userSerno,
                                        villageCode = item.villageCode,
                                        wiringNo = item.wiringNo,
                                        meterBrandCode = item.meterBrandCode,
                                        blockName = item.blockName,
                                        siteName = item.siteName
                                    }).ToList();

                    if(uavtList.Count > 0) {
                        Log.InfoFormat("Fetch operation finished count of list[{0}]",uavtList.Count);
                        resp.Content = new ObjectContent(typeof(List<Uavts>),uavtList,_jsonFormatter);
                    } else {
                        Log.InfoFormat("Fetch operation finished there is no valid data for dcode [{0}] and date [{1}]",reqModel.DistrictCode,reqModel.LastProcessDate);
                        resp.Content = new StringContent("");
                    }
                }

                resp.StatusCode = HttpStatusCode.OK;
            } catch(Exception exc) {
                resp.StatusCode = HttpStatusCode.InternalServerError;
                Log.ErrorFormat("Error occured and exception is [{0}]",JsonConvert.SerializeObject(exc,Formatting.Indented));
            }

            return resp;
        }

        [Deflate]
        [Route("fetchUsers")]
        [HttpPost]
        public HttpResponseMessage FetchUser(dynamic data) {
            var resp = new HttpResponseMessage();
            try {
                Log.InfoFormat("FetchUser request come");

                using(var model = new Entities()) {
                    var users = model.Users.ToList();
                    if(users.Count > 0) {
                        Log.InfoFormat("User fetch operation finished count of list[{0}]",users.Count);
                        resp.Content = new ObjectContent(typeof(List<Users>),users,_jsonFormatter);
                    } else {
                        Log.Info("User fetch operation finished there is no valid data");
                        resp.Content = new StringContent("");
                    }

                }

                resp.StatusCode = HttpStatusCode.OK;
            } catch(Exception exc) {
                Log.ErrorFormat("Error occured and exception is [{0}]",JsonConvert.SerializeObject(exc,Formatting.Indented));
                resp.StatusCode = HttpStatusCode.InternalServerError;
            }

            return resp;
        }

        [Deflate]
        [Route("fetchMbs")]
        [HttpPost]
        public HttpResponseMessage FetchMBS(FetchRequestModel reqModel) {
            var resp = new HttpResponseMessage();
            try {
                Log.InfoFormat("FetchMBS request resp [{0}]",JsonConvert.SerializeObject(reqModel,Formatting.Indented));

                using(var model = new Entities()) {
                    var longVal = long.Parse(reqModel.DistrictCode);
                    var list = new List<ABONE_BILGI>();
                    if(reqModel.LastProcessDate == null) {
                        list = model.ABONE_BILGI.Where(a => a.BOLGE_KODU == longVal || a.BOLGE_KODU == 31).ToList();
                    } else {
                        list = model.ABONE_BILGI.Where(a => (a.BOLGE_KODU == longVal || a.BOLGE_KODU == 31) && a.SOZLESME_TARIHI >= reqModel.LastProcessDate).ToList();
                    }

                    if(list.Count > 0) {
                        Log.InfoFormat("MBS fetch operation finished count of list[{0}]",list.Count);
                        resp.Content = new ObjectContent(typeof(List<ABONE_BILGI>),list,_jsonFormatter);
                    } else {
                        Log.InfoFormat("MBS fetch operation finished there is no valid data for dcode [{0}] and date [{1}]",reqModel.DistrictCode,reqModel.LastProcessDate);
                        resp.Content = new StringContent("");
                    }

                }

                resp.StatusCode = HttpStatusCode.OK;
            } catch(Exception exc) {
                resp.StatusCode = HttpStatusCode.InternalServerError;
                Log.ErrorFormat("Error occured and exception is [{0}]",JsonConvert.SerializeObject(exc,Formatting.Indented));
            }

            return resp;
        }

        [Deflate]
        [Route("fetchLogs")]
        [HttpPost]
        public HttpResponseMessage FetchLogs(FetchRequestModel reqModel) {
            var resp = new HttpResponseMessage();
            try {
                Log.InfoFormat("FetchLogs request come [{0}]",JsonConvert.SerializeObject(reqModel,Formatting.Indented));
                using(var model = new Entities()) {
                    //create date max olanı al
                    long now = Utilities.DateTimeNowLong();
                    if(reqModel.LastProcessDate == null) {
                        reqModel.LastProcessDate = 20121010000000;
                    }

                    var uavts =
                        model.AuditLogs.Where(
                            a => a.ProcessDate >= reqModel.LastProcessDate && a.ProcessDate <= now && a.DistrictCode == reqModel.DistrictCode)
                            .ToList();
                    var mergedVal = uavts.OrderByDescending(x => x.CreateDate)
                        .GroupBy(
                            x =>
                                new {
                                    x.CsbmCode,
                                    x.DistrictCode,
                                    x.DoorNumber,
                                    x.IndoorNumber,
                                    x.StreetCode,
                                    x.VillageCode
                                }).Select(a => new { a,count = a.Count() }).SelectMany(b => b.a.Select(c => c).Zip(Enumerable.Range(1,b.count),
                            (j,i) =>
                                new {
                                    j.CsbmCode,
                                    j.CreateDate,
                                    j.DistrictCode,
                                    j.DoorNumber,
                                    j.IndoorNumber,
                                    j.ProcessDate,
                                    j.StreetCode,
                                    j.UavtCode,
                                    j.UserSerno,
                                    j.VillageCode,
                                    j.BlockName,
                                    j.SiteName,
                                    j.ProgressStatus,
                                    j.OptionSelection,
                                    j.FormSerno,
                                    j.FormDescription,
                                    j.Status,
                                    j.PreviousCheckStatus,
                                    j.IsActive,
                                    rn = i
                                }));
                    var uavtList = (from item in mergedVal
                                    where item.rn == 1
                                    select new AuditLogModel() {
                                        CsbmCode = item.CsbmCode,
                                        CreateDate = (long)item.CreateDate,
                                        DistrictCode = item.DistrictCode,
                                        DoorNumber = item.DoorNumber,
                                        IndoorNumber = item.IndoorNumber,
                                        StreetCode = item.StreetCode,
                                        UavtCode = item.UavtCode,
                                        UserSerno = (long)item.UserSerno,
                                        VillageCode = item.VillageCode,
                                        BlockName = item.BlockName,
                                        SiteName = item.SiteName,
                                        AuditProgressStatus = item.ProgressStatus,
                                        AuditOptionSelection = item.OptionSelection,
                                        AuditFormSerno = item.FormSerno,
                                        AuditFormDescription = item.FormDescription,
                                        AuditStatus = item.Status,
                                        AuditedCheckStatus = item.PreviousCheckStatus,
                                        RecordStatus = item.IsActive == true ? 0 : 1
                                    }).ToList();

                    if(uavtList.Count > 0) {
                        Log.InfoFormat("FetchLogs operation finished count of list[{0}]",uavtList.Count);
                        resp.Content = new ObjectContent(typeof(List<AuditLogModel>),uavtList,_jsonFormatter);
                    } else {
                        Log.InfoFormat("FetchLogs operation finished there is no valid data for dcode [{0}] and date [{1}]",reqModel.DistrictCode,reqModel.LastProcessDate);
                        resp.Content = new StringContent("");
                    }
                }

                resp.StatusCode = HttpStatusCode.OK;
            } catch(Exception exc) {
                resp.StatusCode = HttpStatusCode.InternalServerError;
                Log.ErrorFormat("Error occured and exception is [{0}]",JsonConvert.SerializeObject(exc,Formatting.Indented));
            }

            return resp;
        }

        [Deflate]
        [Route("fetchNew")]
        [HttpPost]
        public HttpResponseMessage FetchNewUavt(FetchRequestModel reqModel) {
            var resp = new HttpResponseMessage();
            try {
                Log.InfoFormat("Fetch new uavt item form uavt_63 request come at [{0}]",Utilities.DateTimeNowLong());
                using(var model = new Entities()) {
                    var newlyCreatedUavt = model.uavt_63.Where(a => a.CSBM_KODU.Contains("-") && a.ILCE_KODU == reqModel.DistrictCode).ToList();
                    if(newlyCreatedUavt.Count > 0) {
                        var listUavt = new List<UavtModel>();
                        newlyCreatedUavt.ForEach(a => listUavt.Add(new UavtModel {
                            BlockName = a.BLOK_ADI,
                            DistrictCode = a.BUCAK_KODU,
                            BuildingCode = a.BINA_KODU,
                            CSBMCode = a.CSBM_KODU,
                            CSBMName = a.CSBM_ADI,
                            CheckStatus = 0,
                            CountyCode = a.ILCE_KODU,
                            CountyName = a.ILCE_ADI,
                            DistrictName = a.BUCAK_ADI,
                            DoorNumber = a.DIS_KAPI_NO,
                            IndoorNumber = a.IC_KAPI_NO,
                            SiteName = a.SITE_ADI,
                            StreetCode = a.MAHALLE_KODU,
                            StreetName = a.MAHALLE_ADI,
                            UAVTAddressNo = a.UAVT_ADRES_NO,
                            VillageCode = a.KOY_KODU,
                            VillageName = a.KOY_ADI
                        }));
                        Log.InfoFormat("MBS fetch operation finished count of list[{0}]",listUavt.Count);
                        resp.Content = new ObjectContent(typeof(List<UavtModel>),listUavt,new JsonMediaTypeFormatter());
                    } else {
                        Log.Info("MBS fetch operation finished there is no valid data");
                        resp.Content = new StringContent("");
                    }

                    resp.StatusCode = HttpStatusCode.OK;
                }
            } catch(Exception exc) {
                resp.StatusCode = HttpStatusCode.InternalServerError;
                Log.ErrorFormat("Error occured and exception is [{0}]",JsonConvert.SerializeObject(exc,Formatting.Indented));
            }

            return resp;
        }

        private void CreateOrUpdateLogs(string auditLog,Entities model) {
            var logModel = JsonConvert.DeserializeObject(auditLog,typeof(AuditLogModel)) as AuditLogModel;
            var existedItem = model.AuditLogs.FirstOrDefault(a => a.UavtCode == logModel.UavtCode && a.CreateDate == logModel.CreateDate);
            if(existedItem != null) {
                existedItem.FormDescription = logModel.AuditFormDescription;
                existedItem.FormSerno = logModel.AuditFormSerno;
                existedItem.OptionSelection = logModel.AuditOptionSelection;
                existedItem.ProgressStatus = logModel.AuditProgressStatus;
                existedItem.PreviousCheckStatus = logModel.AuditedCheckStatus;
                existedItem.Status = logModel.AuditStatus;
                model.Entry(existedItem).State = EntityState.Modified;
            } else {
                model.AuditLogs.Add(new AuditLogs {
                    BlockName = logModel.BlockName,
                    CreateDate = logModel.CreateDate,
                    CsbmCode = logModel.CsbmCode,
                    DistrictCode = logModel.DistrictCode,
                    DoorNumber = logModel.DoorNumber,
                    FormDescription = logModel.AuditFormDescription,
                    FormSerno = logModel.AuditFormSerno,
                    IndoorNumber = logModel.IndoorNumber,
                    IsActive = true,
                    OptionSelection = logModel.AuditOptionSelection,
                    ProcessDate = Utilities.DateTimeNowLong(),
                    ProgressStatus = logModel.AuditProgressStatus,
                    Serno = logModel.CreateDate,
                    SiteName = logModel.SiteName,
                    StreetCode = logModel.StreetCode,
                    UavtCode = logModel.UavtCode,
                    UserSerno = logModel.UserSerno,
                    VillageCode = logModel.VillageCode,
                    PreviousCheckStatus = logModel.AuditedCheckStatus,
                    Status = logModel.AuditStatus
                });
            }
        }

        private void CreateOrUpdateUavt(string uavtModelItem,Entities model) {
            var uavtModel = JsonConvert.DeserializeObject(uavtModelItem,typeof(UavtDataModel)) as UavtDataModel;

            var existedItem = model.Uavts.FirstOrDefault(a => a.uavtCode == uavtModel.UavtCode && a.createDate == uavtModel.CreateDate);
            if(existedItem != null) {
                existedItem.wiringNo = uavtModel.WiringNo;
                existedItem.customerName = uavtModel.CustomerName;
                existedItem.meterBrand = uavtModel.MeterBrand;
                existedItem.meterNo = uavtModel.MeterNo;
                existedItem.checkStatus = uavtModel.CheckStatus;
                existedItem.meterBrandCode = uavtModel.MeterBrandCode;
                existedItem.siteName = uavtModel.SiteName;
                existedItem.blockName = uavtModel.BlockName;
                model.Entry(existedItem).State = EntityState.Modified;
            } else {
                model.Uavts.Add(new Uavts {
                    checkStatus = uavtModel.CheckStatus,
                    createDate = uavtModel.CreateDate,
                    customerName = uavtModel.CustomerName,
                    districtCode = uavtModel.DistrictCode,
                    doorNumber = uavtModel.DoorNumber,
                    existOnUavt = uavtModel.ExistOnUavt,
                    meterBrand = uavtModel.MeterBrand,
                    meterNo = uavtModel.MeterNo,
                    uavtCode = uavtModel.UavtCode,
                    userSerno = uavtModel.UserSerno,
                    wiringNo = uavtModel.WiringNo,
                    villageCode = uavtModel.VillageCode,
                    csbmCode = uavtModel.CsbmCode,
                    indoorNumber = uavtModel.IndoorNumber,
                    streetCode = uavtModel.StreetCode,
                    meterBrandCode = uavtModel.MeterBrandCode,
                    siteName = uavtModel.SiteName,
                    blockName = uavtModel.BlockName,
                    processDate = Utilities.DateTimeNowLong()
                });
            }
        }

    }
}
