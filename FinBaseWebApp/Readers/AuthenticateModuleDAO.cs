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

