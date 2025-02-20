using Dapper;
using FinBaseWebApp.Models;
using FinBaseWebApp.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FinBaseWebApp.Readers
{
    public class LogDAO
    {
        private readonly string _connectionString;      

        public LogDAO()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["FinBaseDB"].ConnectionString;      
        }       

        public void saveLog(ApiLogs log)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string query = LogQueries.INSERT_INTO_APILOGS;
                dbConnection.Query(query, new 
                {
                    @Controller = log.Controller,   
                    @MethodName = log.MethodName,   
                    @UserId = log.UserId,   
                    @Parameters = log.Parameters,   
                    @PostData = log.PostData,   
                    @CreatedBy = log.CreatedBy, 
                    @CreatedAt = log.CreatedAt,     
                    @ModifiedBy = log.ModifiedBy,   
                    @ModifiedAt = log.ModifiedAt,   
                    @Status = log.Status    
                });    
            }
        }   
    }
}

