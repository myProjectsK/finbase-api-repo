using Dapper;
using FinBaseWebApp.Models;
using FinBaseWebApp.Queries;     
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FinBaseWebApp.Helpers
{
    public class UploadHelper
    {

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["FinBaseDB"].ConnectionString;

        public bool ValidateUserAccessToFile(string UserName, string fileName)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string query = PublicUserQueries.GET_USER_AUTHORIZATION;    
                // Query the database to ensure the file belongs to the user
                var userFile = dbConnection.QuerySingleOrDefault<string>(query ,new { UserName, fileName });

                return !string.IsNullOrEmpty(userFile);
            }
        }

            /* public string FindDocumentType(string fileName, List<VerifyDocumentType> documentTypes)
                {

                    if (documentTypes == null || !documentTypes.Any())
                    {
                        throw new ArgumentException("Document type list is empty or null.", nameof(documentTypes));
                    }

                    // Check if file name contains "Aadhaar"
                    if (fileName.IndexOf("Aadhaar", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return documentTypes.FirstOrDefault(dt => dt.DocumentType.IndexOf("Aadhaar", StringComparison.OrdinalIgnoreCase) >= 0)?.DocumentType ?? "Other";
                    }
                    // Check if file name contains "PAN"
                    else if (fileName.IndexOf("PAN", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return documentTypes.FirstOrDefault(dt => dt.DocumentType.IndexOf("PAN", StringComparison.OrdinalIgnoreCase) >= 0)?.DocumentType ?? "Other";
                    }
                    // Fallback or default document type
                    return "Other";
                }  */

        /*public List<VerifyDocumentType> GetDocumentTypes()
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                string query = "SELECT DocumentType FROM VerifyDocumentType WHERE Status = 1";
                return dbConnection.Query<VerifyDocumentType>(query).ToList();
            }
        }*/
    }
}

