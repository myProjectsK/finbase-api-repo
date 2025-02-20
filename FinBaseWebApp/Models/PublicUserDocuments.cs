using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Models
{
    public class PublicUserDocuments    
    {   
        public string MobileNo { get; set; }    

        [JsonProperty("Email")]
        public string EmailId { get; set; }
        public int DocumentId { get; set; }
        public string DocumentType { get; set; }    
        public string DocumentFileName { get; set; }    
        public string CreatedBy { get; set; }   
        public DateTime CreatedAt { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool Status { get; set; }
    }
}   

