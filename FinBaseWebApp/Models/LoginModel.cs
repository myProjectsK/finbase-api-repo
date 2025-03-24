using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Models
{
    public class LoginModel
    {
        public string UserId { get; set; }
        [JsonProperty("Mobile")]
        public string RoleName { get; set; }
        public string MobileNo { get; set; }
        public string Name { get; set; }
        [JsonProperty("Email")]
        public string EmailId { get; set; }
        public string PasswordHash { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool Status { get; set; }
    }
}
