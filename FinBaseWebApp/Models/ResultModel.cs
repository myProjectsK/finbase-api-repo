using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Models
{
    public class ResultModel
    {

        public bool Success { get; set; }       
        public object Data { get; set; }        
        public string ErrorMessage { get; set; }    
        public string TechDetails { get; set; }
    }
}

