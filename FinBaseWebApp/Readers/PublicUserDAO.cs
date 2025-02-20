using FinBaseWebApp.Models;
using FinBaseWebApp.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using System.Threading.Tasks;

namespace FinBaseWebApp.Readers
{
    public class PublicUserDAO
    {

        private readonly string _connectionString;      

        public PublicUserDAO()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["FinBaseDB"].ConnectionString;       
        }

        /*public List<PublicUserModel> GetAllPublicUsers()
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();

                    var query = PublicUserQueries.GET_ALL_PUBLICUSERS;
                    return dbConnection.Query<PublicUserModel>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                return null;    
            }
        }   */

        public async Task<PublicUserModel> Public_LoginUser(string UserName, string Password)
        {
           /* try
            {*/
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();

                    string query = PublicUserQueries.GET_PUBLICUSER_FROM_USERNAME_AND_PASSWORD;
                    return await dbConnection.QueryFirstOrDefaultAsync<PublicUserModel>(query, new
                    {
                        @UserName = UserName,
                        @Password = Password
                    });
                }
            /*}
            catch (Exception) - Must declare the scalar variable "@@UserName".
            {
                return null;
            }*/
        }

        public PublicUserModel GetPublicUser(string UserName)
        {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();
   
                    var query = PublicUserQueries.GET_USER_DETAILS;       
                    return dbConnection.QueryFirstOrDefault<PublicUserModel>(query, new { @UserName = UserName });     
                }
        }

        public PublicUserModel GetUserProfile(string UserName)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                Console.WriteLine($"Executing query with UserName: {UserName}");    
                var query = PublicUserQueries.GET_USER_DETAILS;
                var userDictionary = new Dictionary<string, PublicUserModel>();     
                var result = dbConnection.Query<PublicUserModel, PublicUserDocuments, PublicUserModel>(query, (user, document) =>
                    {
                        // Check if the user already exists in the dictionary
                        if (!userDictionary.TryGetValue(user.MobileNo, out var userEntry))
                        {
                            userEntry = new PublicUserModel
                            {
                                MobileNo = user.MobileNo,
                                Name = user.Name,
                                DateOfBirth = user.DateOfBirth,
                                Gender = user.Gender,
                                Address = user.Address,
                                EmailId = user.EmailId,
                                DocumentFiles = new List<PublicUserDocuments>() // Initialize document list
                            };
                            userDictionary.Add(user.MobileNo, userEntry);
                        }

                        // Add the document if it's not null
                        if (document != null && !string.IsNullOrEmpty(document.DocumentFileName))
                        {
                            userEntry.DocumentFiles.Add(new PublicUserDocuments
                            {
                                DocumentFileName = document.DocumentFileName
                            });
                        }

                        return userEntry;
                    },
                    splitOn: "DocumentFileName",
                    param: new { @UserName = UserName }
                );

                return userDictionary.Values.FirstOrDefault();
            }
        }

        public bool InsertPublicUser(PublicUserModel user)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                var query = PublicUserQueries.INSERT_INTO_PUBLICUSER;
                var row = dbConnection.Execute(query, new
                {       
                    @MobileNo = user.MobileNo,
                    @Name = user.Name,
                    @DOB = user.DateOfBirth,   
                    @Gender =  user.Gender,    
                    @Address = user.Address,
                    @EmailId = user.EmailId,
                    @Password = user.Password,
                    @CreatedBy = user.CreatedBy,
                    @CreatedAt = user.CreatedAt,
                    @ModifiedBy = user.ModifiedBy,
                    @ModifiedAt = user.ModifiedAt,
                    @Status = user.Status
                });

                return row > 0;          
            }
        }

        public bool InsertPublicUserDocument(PublicUserDocuments document)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var query = PublicUserQueries.INSERT_INTO_PUBLICUSERDOCUMENTS;
                var row = dbConnection.Execute(query, new
                {
                    @MobileNo = document.MobileNo,
                    @DocumentFileName = document.DocumentFileName,
                    @CreatedBy = document.CreatedBy,
                    @CreatedAt = document.CreatedAt,
                    @ModifiedBy = document.ModifiedBy,
                    @ModifiedAt = document.ModifiedAt,
                    @Status = document.Status
                });

                return row > 0;
            }
        }

        public bool UpdatePublicUser(PublicUserModel user, string userName)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string query = PublicUserQueries.UPDATE_PUBLICUSER;
                var row = dbConnection.Execute(query, new
                {
                    @UserName = userName,   
                    @MobileNo = user.MobileNo,
                    @Name = user.Name,
                    @DOB = user.DateOfBirth,
                    @Gender = user.Gender,  
                    @Address = user.Address,
                    @EmailId = user.EmailId,    
                    @ModifiedBy = user.ModifiedBy,
                    @ModifiedAt = user.ModifiedAt,
                    @Status = user.Status
                });

                return row > 0;      
            }
        }

        public bool UpdatePublicUserDocument(PublicUserDocuments document, string userName)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();    
                var query = PublicUserQueries.UPDATE_PUBLICUSERDOCUMENTS;    
                var row = dbConnection.Execute(query, new
                {
                    @UserName = userName,
                    @MobileNo = document.MobileNo,
                    @EmailId = document.EmailId,    
                    @DocumentType = document.DocumentType,  
                    @DocumentFileName = document.DocumentFileName,  
                    @ModifiedBy = document.ModifiedBy,  
                    @ModifiedAt = document.ModifiedAt,  
                    @Status = document.Status   
                }); 

                return row > 0;
            }
        }

        public bool DeletePublicUser(string Mobile, string LoginUserId)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string query = PublicUserQueries.DELETE_PUBLICUSER;
                int rowsAffected = dbConnection.Execute(query, new
                {
                    @MobileNo = Mobile,
                    @Modifiedby = LoginUserId,
                    @ModifiedAt = DateTime.Now
                });

                return rowsAffected > 0;    
            }      
        }       
    }
}

