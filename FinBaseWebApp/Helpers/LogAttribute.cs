using FinBaseWebApp.Models;
using FinBaseWebApp.Repository;
using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FinBaseWebApp.Helpers
{
    public class LogAttribute : ActionFilterAttribute
    {
        private readonly LogRepository _logRepo;    

        public LogAttribute()
        {
            _logRepo = new LogRepository();     
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {    
            ApiLogs obj = new ApiLogs();

            obj.Controller = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            obj.MethodName = actionContext.ActionDescriptor.ActionName;
            obj.UserId = HttpContext.Current.User.Identity.Name;
            obj.Parameters = string.Join(",", actionContext.ActionArguments.Keys.ToList());
            obj.PostData = JsonConvert.SerializeObject(actionContext.ActionArguments);
            obj.CreatedBy = GetClientIpAddress(actionContext.Request);
            obj.CreatedAt = DateTime.Now;   
            obj.ModifiedBy = GetClientIpAddress(actionContext.Request);
            obj.ModifiedAt = obj.CreatedAt;
            obj.Status = true;  
        }

        private string GetClientIpAddress(HttpRequestMessage request)
        {   
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return IPAddress.Parse(((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress).ToString();         
            }

            if (request.Properties.ContainsKey("MS_OwinContext"))
            {
                return IPAddress.Parse(((OwinContext)request.Properties["MS_OwinContext"]).Request.RemoteIpAddress).ToString();
            }

            return string.Empty;    
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Trace.WriteLine(string.Format("Action Method {0} executed at {1}", actionExecutedContext.ActionContext.ActionDescriptor.ActionName, DateTime.Now.ToShortDateString()), "Web API Logs");      
        }
    }
}

