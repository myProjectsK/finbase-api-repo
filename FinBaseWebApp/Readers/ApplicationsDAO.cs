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
    public class ApplicationsDAO
    {
        private readonly string _connectionString;      

        public ApplicationsDAO()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["FinBaseDB"].ConnectionString;
        }

        public ApplicationsModel GetApplicationDAO(string mobile)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var query = ApplicationQueries.GET_APPLICATION_DETAILS;         
                var userDictionary = new Dictionary<string, ApplicationsModel>();
                var result = dbConnection.Query<ApplicationsModel, ApplicationFilesModel, ApplicationsModel>(query, (user, file) =>
                {
                    // Check if the user already exists in the dictionary
                    if (!userDictionary.TryGetValue(user.MobileNo, out var userEntry))
                    {
                        userEntry = new ApplicationsModel     
                        {
                            ApplicationNo = user.ApplicationNo, 
                            Name = user.Name,   
                            MobileNo = user.MobileNo,       
                            DateOfBirth = user.DateOfBirth,
                            Gender = user.Gender,
                            OccupationType = user.OccupationType,   
                            OccupationName = user.OccupationName,   
                            LoanType = user.LoanType,   
                            Amount = user.Amount,   
                            RateOfInterest = user.RateOfInterest,   
                            CreatedAt = user.CreatedAt, 
                            DocumentFiles = new List<ApplicationFilesModel>()   
                        };
                        userDictionary.Add(user.MobileNo, userEntry);
                    }

                    // Add the document if it's not null
                    if (file != null && !string.IsNullOrEmpty(file.DocumentFileName))
                    {
                        userEntry.DocumentFiles.Add(new ApplicationFilesModel    
                        {
                            DocumentFileName = file.DocumentFileName    
                        });     
                    }   

                    return userEntry;
                },
                    splitOn: "DocumentFileName",
                    param: new { @Phone = mobile }
                );

                return userDictionary.Values.FirstOrDefault();
            }
        }

        public bool InsertApplicationDAO(ApplicationsModel user)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var query = ApplicationQueries.INSERT_INTO_APPLICATIONS;    
                var row = dbConnection.Execute(query, new
                {
                    @AppNo = user.ApplicationNo,    
                    @Name = user.Name,  
                    @MobileNo = user.MobileNo,  
                    @DOB = user.DateOfBirth,    
                    @Gender = user.Gender,  
                    @WorkType = user.OccupationType,
                    @WorkName = user.OccupationName,   
                    @loanType = user.LoanType,  
                    @Amt = user.Amount,    
                    @ROI = user.RateOfInterest,    
                    @CreatedBy = user.CreatedBy,
                    @CreatedAt = user.CreatedAt,
                    @ModifiedBy = user.ModifiedBy,
                    @ModifiedAt = user.ModifiedAt,
                    @Status = user.Status
                });

                return row > 0;
            }
        }

        public bool InsertApplicationFileDAO(ApplicationFilesModel file)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var query = ApplicationQueries.INSERT_INTO_APPLICATIONFILES;     
                var row = dbConnection.Execute(query, new
                {
                    @MobileNo = file.MobileNo,  
                    @fileType = file.FileType,  
                    @DocumentFileName = file.DocumentFileName,
                    @CreatedBy = file.CreatedBy,
                    @CreatedAt = file.CreatedAt,
                    @ModifiedBy = file.ModifiedBy,
                    @ModifiedAt = file.ModifiedAt,
                    @Status = file.Status
                });

                return row > 0;
            }
        }

        public bool UpdateApplicationDAO(ApplicationsModel user, string mobile)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string query = ApplicationQueries.UPDATE_APPLICATIONS;    
                var row = dbConnection.Execute(query, new   
                {   
                    @Phone = mobile,    
                    @MobileNo = user.MobileNo,
                    @Name = user.Name,
                    @DOB = user.DateOfBirth,
                    @Gender = user.Gender,
                    @WorkType = user.OccupationType,
                    @WorkName = user.OccupationName,
                    @loanType = user.LoanType,
                    @Amt = user.Amount,
                    @ROI = user.RateOfInterest,
                    @ModifiedBy = user.ModifiedBy,
                    @ModifiedAt = user.ModifiedAt,
                    @Status = user.Status
                });

                return row > 0;
            }
        }

        public bool UpdateApplicationFileDAO(ApplicationFilesModel file, string mobile)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var query = ApplicationQueries.UPDATE_APPLICATIONFILES;        
                var row = dbConnection.Execute(query, new   
                {   
                    @Phone = mobile,       
                    @MobileNo = file.MobileNo,  
                    @fileType = file.FileType,  
                    @DocumentFileName = file.DocumentFileName,
                    @ModifiedBy = file.ModifiedBy,
                    @ModifiedAt = file.ModifiedAt,
                    @Status = file.Status
                });

                return row > 0;
            }
        }

        public bool DeleteApplicationDAO(string mobile)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string query = ApplicationQueries.DELETE_APPLICATIONS;      
                int rowsAffected = dbConnection.Execute(query, new
                {
                    @MobileNo = mobile,     
                    @ModifiedAt = DateTime.Now
                });

                return rowsAffected > 0;
            }
        }   
    }   
}   
