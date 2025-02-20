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
    public class PublicUserLoanDAO
    {
        private readonly string _connectionString;      

        public PublicUserLoanDAO()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["FinBaseDB"].ConnectionString;
        }

        public PublicUserLoanModel GetUserLoanInfo(string mobile)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var query = PublicUserLoanQueries.GET_USERLOAN_DETAILS;     
                var userDictionary = new Dictionary<string, PublicUserLoanModel>();
                var result = dbConnection.Query<PublicUserLoanModel, PublicUserLoanFilesModel, PublicUserLoanModel>(query, (user, file) =>
                {
                    // Check if the user already exists in the dictionary
                    if (!userDictionary.TryGetValue(user.MobileNo, out var userEntry))
                    {
                        userEntry = new PublicUserLoanModel     
                        {
                            Name = user.Name,   
                            MobileNo = user.MobileNo,       
                            DateOfBirth = user.DateOfBirth,
                            Gender = user.Gender,
                            OccupationType = user.OccupationType,   
                            OccupationName = user.OccupationName,   
                            LoanType = user.LoanType,   
                            Amount = user.Amount,   
                            RateOfInterest = user.RateOfInterest,   
                            DocumentFiles = new List<PublicUserLoanFilesModel>() // Initialize document list
                        };
                        userDictionary.Add(user.MobileNo, userEntry);
                    }

                    // Add the document if it's not null
                    if (file != null && !string.IsNullOrEmpty(file.DocumentFileName))
                    {
                        userEntry.DocumentFiles.Add(new PublicUserLoanFilesModel    
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

        public bool InsertPublicUserLoan(PublicUserLoanModel user)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var query = PublicUserLoanQueries.INSERT_INTO_PUBLICUSERLOAN;   
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

        public bool InsertPublicUserLoanFile(PublicUserLoanFilesModel file)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var query = PublicUserLoanQueries.INSERT_INTO_PUBLICUSERLOANFILES;   
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

        public bool UpdatePublicUserLoan(PublicUserLoanModel user, string mobile)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string query = PublicUserLoanQueries.UPDATE_PUBLICUSERLOAN;     
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

        public bool UpdatePublicUserLoanFile(PublicUserLoanFilesModel file, string mobile)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();    
                var query = PublicUserLoanQueries.UPDATE_PUBLICUSERLOANFILES;   
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

        public bool DeletePublicUserLoan(string mobile)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string query = PublicUserLoanQueries.DELETE_PUBLICUSERLOAN;     
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
