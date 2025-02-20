using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Models
{
    public class PublicUserModel
    {
        public string PublicUserId { get; set; }    

        [JsonProperty("Mobile")]
        public string MobileNo { get; set; }
        public string Name { get; set; }    
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }

        [JsonProperty("Email")]      
        public string EmailId { get; set; }    
        public string Password { get; set; }
        public List<PublicUserDocuments> DocumentFiles { get; set; } = new List<PublicUserDocuments>();      
        public string CreatedBy { get; set; }   
        public DateTime CreatedAt { get; set; }     
        public String ModifiedBy { get; set; }      
        public DateTime ModifiedAt { get; set; }    
        public bool Status { get; set; }    
    }   
}       

