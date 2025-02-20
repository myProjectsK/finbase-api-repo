using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Models
{
    public class PublicUserLoanModel
    {
        public string PublicUserLoanId { get; set; }    
        public string ApplicationNo { get; set; }   
        public string Name { get; set; }        

        [JsonProperty("Mobile")]
        public string MobileNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string OccupationType { get; set; }     
        public string OccupationName { get; set; }
        public string LoanType { get; set; }
        public long Amount { get; set; }
        public double RateOfInterest { get; set; }
        public List<PublicUserLoanFilesModel> DocumentFiles { get; set; } = new List<PublicUserLoanFilesModel>();
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool Status { get; set; }    
    }
}   
