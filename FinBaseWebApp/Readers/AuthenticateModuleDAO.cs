using FinBaseWebApp.Models;
using FinBaseWebApp.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;      

namespace FinBaseWebApp.Readers
{
    public class AuthenticateModuleDAO
    {

        private readonly string _connectionString;      

        public AuthenticateModuleDAO()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["FinBaseDB"].ConnectionString;       
        }

        public async Task<LoginModel> GetUserDAO(string UserName)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();

                    string query = AuthenticateModuleQueries.GET_USER_DETAILS;      
                    var result = await dbConnection.QueryFirstOrDefaultAsync<dynamic>(query, new
                    {
                        @UserName = UserName
                    });
                    if (result == null)
                        return null;

                    var user = new LoginModel
                    {
                        UserId = result.UserId.ToString(),
                        EmailId = result.EmailId,
                        Name = result.Name,     
                        RoleName = result.RoleName
                    };  

                    return user;
                }
            }
            catch (Exception ex)
            {
                var details = ex.Message + Environment.NewLine + ex.ToString() + Environment.NewLine + (ex.InnerException == null ? "" : ex.InnerException.ToString());
                throw;
            }
        }

    public async Task<LoginModel> Public_LoginUser(string UserName, string Password)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();      

                    string query = AuthenticateModuleQueries.GET_LOGINUSER_FROM_USERNAME_AND_PASSWORD;
                    var result = await dbConnection.QueryFirstOrDefaultAsync<dynamic>(query, new
                    {
                        @UserName = UserName
                    });     
                    if (result == null)
                        return null;

                    var user = new LoginModel
                    {
                        UserId = result.UserId.ToString(),  
                        MobileNo = result.MobileNo, 
                        EmailId = result.EmailId,   
                        Name = result.Name, 
                        PasswordHash = result.PasswordHash, 
                        RoleName = result.RoleName  
                    };  

                    if (!BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
                        return null;

                    return user;
                }
            }
            catch (Exception ex)
            {
                var details = ex.Message + Environment.NewLine + ex.ToString() + Environment.NewLine + (ex.InnerException == null ? "" : ex.InnerException.ToString());     
                throw;
            }
        }

        public async Task<List<RefreshTokenModel>> GetAllTokens()
        {
            try
            {
                using(IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();        

                    var query = AuthenticateModuleQueries.GET_ALL_REFRESHTOKEN_DETAILS;     
                    return (await dbConnection.QueryAsync<RefreshTokenModel>(query)).ToList();      
                }
            }
            catch(Exception ex)
            {
                return null;    
            }
        }

        public async Task<RefreshTokenModel> GetTokenById(string tokenid)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();

                    string query = AuthenticateModuleQueries.GET_TOKEN_BY_ID;
                    return await dbConnection.QueryFirstOrDefaultAsync<RefreshTokenModel>(query, new { @ID = tokenid });    
                }
            }
            catch (Exception ex)
            {
                return null;
            }   
        }   

        public async Task<bool> CheckTokenById(string tokenId)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string query = AuthenticateModuleQueries.CHECK_TOKEN_BY_ID;     
                return dbConnection.Query<RefreshTokenModel>(query, new { @tokenid = tokenId }).Any();    
            }
        }

        public async Task<bool> CheckTokenByUserName(RefreshTokenModel refreshToken)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string query = AuthenticateModuleQueries.CHECK_TOKEN_BY_USERNAME;    
                return dbConnection.Query<RefreshTokenModel>(query, new { @Username = refreshToken.USERNAME }).Any();
            }
        }       

        public async Task<bool> DeleteRefreshTokenById(string tokenid)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();    

                string query = AuthenticateModuleQueries.DELETE_REFRESH_TOKEN_FROM_ID;
                int rowsaffected = await dbConnection.ExecuteAsync(query, new { @Id = tokenid });

                return rowsaffected > 0;      
            }
        }

        public async Task<string> InsertRefreshToken(RefreshTokenModel user)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();

                string query = AuthenticateModuleQueries.INSERT_REFRESH_TOKEN;
                string Id = await dbConnection.QueryFirstOrDefaultAsync<string>(query, new
                {
                    @Id = user.TOKENID,
                    @UserName = user.USERNAME,
                    @IssuedDateTime = user.ISSUEDDATETIME,
                    @ExpiredDateTime = user.EXPIREDDATETIME,
                    @ProtectedTicket = user.PROTECTEDTICKET
                });

                return Id;
            }
        }   
    }
}

