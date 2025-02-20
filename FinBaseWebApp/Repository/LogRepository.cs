using FinBaseWebApp.Models;
using FinBaseWebApp.Readers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Repository
{
    public class LogRepository
    {
        private readonly LogDAO _logDAO;    

        public LogRepository()
        {
            _logDAO = new LogDAO();     
        }   

        public void SaveLogInfo(ApiLogs Log)
        {
            _logDAO.saveLog(Log);   
        }
    }
}
