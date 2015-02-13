using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web;
using System.Web.Http;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Modem.Map.Controllers {
    [RoutePrefix("api/workapi")]
    public class WorkApiController : ApiController {
        readonly MediaTypeFormatter _jsonFormatter = new JsonMediaTypeFormatter {
            SerializerSettings = {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            }
        };

        [Route("upload")]
        [HttpPost]
        public HttpResponseMessage Post() {
            var resp = new HttpResponseMessage();
            try {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];

                var assemblyPath = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
                var directoryPath = Path.GetDirectoryName(assemblyPath);

                if(httpRequest.Files.Count > 0) {
                    if(directoryPath != null)
                        using(var fileStream = File.Create(Path.Combine(directoryPath,postedFile.FileName))) {
                            postedFile.InputStream.CopyTo(fileStream);
                        }
                    //var data = ReadData();
                    //data.Add("fileName",Path.Combine(directoryPath,postedFile.FileName));

                    //result.Content = new ObjectContent(typeof(Dictionary<object,object>),data,_jsonFormatter);
                    //result.StatusCode = HttpStatusCode.OK;

                    var fileName = Path.Combine(directoryPath,postedFile.FileName);

                    if(!File.Exists(fileName)) {
                    }

                    var workbook = new XLWorkbook(fileName);
                    var worksheet = workbook.Worksheet(1);
                    var data = GetData(worksheet,21);
                    resp.Content = new ObjectContent(typeof(List<Dictionary<object,object>>),data,_jsonFormatter);
                }
                resp.StatusCode = HttpStatusCode.OK;
            } catch(Exception) {
                resp.StatusCode = HttpStatusCode.InternalServerError;
            }

            return resp;
        }

        private static List<Dictionary<object,object>> GetData(IXLWorksheet worksheet,int count) {
            var dataset = new List<Dictionary<object,object>>();

            var columnList = worksheet.Row(1).Cells().Select(xlCell => xlCell.GetValue<string>()).ToList();
            var length = count == 20 ? worksheet.Rows().Take(count).Count() : worksheet.Rows().Count();
            for(int l = 2; l <= length; l++) {
                var objectDictionary = new Dictionary<object,object>();
                for(int c = 0; c < columnList.Count; c++) {
                    var cellCounter = c + 1;
                    objectDictionary.Add(columnList.ElementAt(c),worksheet.Row(l).Cell(cellCounter).GetString());
                }
                dataset.Add(objectDictionary);
            }

            return dataset;
        }

    }
}
