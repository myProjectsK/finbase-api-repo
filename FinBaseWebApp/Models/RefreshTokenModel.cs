using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Models
{
    public class RefreshTokenModel
    {   

        public string TOKENID { get; set; }     
        public string USERNAME { get; set; }        
        public DateTime? ISSUEDDATETIME { get; set; }       
        public DateTime? EXPIREDDATETIME { get; set; }      
        public string PROTECTEDTICKET { get; set; }
    }
}

