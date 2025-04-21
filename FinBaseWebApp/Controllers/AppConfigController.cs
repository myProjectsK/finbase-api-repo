using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

namespace FinBaseWebApp.Controllers
{
    [RoutePrefix("api/config")] 
    public class AppConfigController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAppConfig()
        {
            // Create a configuration object to return to the client
            var config = new
            {
                ApiBaseUrl = ConfigurationManager.AppSettings["JwtIssuer"],
                Features = new
                {
                    EnableNotifications = bool.Parse(ConfigurationManager.AppSettings["EnableNotifications"] ?? "false"),
                    EnableDarkMode = bool.Parse(ConfigurationManager.AppSettings["EnableDarkMode"] ?? "false")
                },
                Timeouts = new
                {
                    SessionTimeout = int.Parse(ConfigurationManager.AppSettings["JwtExpiryMinutes"] ?? "1440")
                }
            };

            return Ok(config);
        }   
    }   
}   
