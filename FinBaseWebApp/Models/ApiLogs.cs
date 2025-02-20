using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Models
{
    public class ApiLogs
    {
        public int Id { get; set; }
        public string Controller { get; set; }
        public string MethodName { get; set; }
        public string UserId { get; set; }
        public string Parameters { get; set; }
        public string PostData { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool Status { get; set; }    
    }
}

