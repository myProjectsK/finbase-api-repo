using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Models
{
    public class VerifyDocumentType
    {
        public string DocumentType { get; set; }    
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool Status { get; set; }
    }
}

