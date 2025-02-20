using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Models
{
    public class PublicUserLoanFilesModel
    {
        public string MobileNo { get; set; }
        public int FileNo { get; set; }
        public string FileType { get; set; }
        public string DocumentFileName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public String ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool Status { get; set; }
    }
}
