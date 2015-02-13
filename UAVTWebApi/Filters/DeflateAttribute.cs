using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace UAVTWebapi.Filters {
    public class DeflateAttribute : ActionFilterAttribute {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext) {
            var content = actionExecutedContext.Response.Content;
            var bytes = content == null ? null : content.ReadAsByteArrayAsync().Result;
            var zlibedContent = bytes == null ? new byte[0] : Utils.Utilities.DeflateByte(bytes);
            actionExecutedContext.Response.Content = new ByteArrayContent(zlibedContent);
            actionExecutedContext.Response.Content.Headers.Remove("Content-Type");
            actionExecutedContext.Response.Content.Headers.Add("Content-encoding","deflate");
            actionExecutedContext.Response.Content.Headers.Add("Content-Type","application/json");
            
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}