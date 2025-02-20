using FinBaseWebApp.Models;
using FinBaseWebApp.Readers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FinBaseWebApp.Repository
{
    public class AuthenticationRepository : IDisposable
    {
        private readonly string _connectionString;      

        public AuthenticationRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["FinBaseDB"].ConnectionString;     
        }

        public async Task<List<RefreshTokenModel>> GetAllRefreshToken()
        {
            var repo = new AuthenticateModuleDAO();
            return await repo.GetAllTokens();     
        }

        public async Task<RefreshTokenModel> GetRefreshTokenById(string tokenId)
        {
            var repo = new AuthenticateModuleDAO();
            return await repo.GetTokenById(tokenId);     
        }

        public async Task<bool> AddRefreshToken(RefreshTokenModel refreshToken)
        {
            var repo = new AuthenticateModuleDAO();

            if (await repo.CheckTokenByUserName(refreshToken))    
            {   
                var result = await repo.DeleteRefreshTokenById(refreshToken.TOKENID);     
            }

            var Id = await repo.InsertRefreshToken(refreshToken);
            return !string.IsNullOrWhiteSpace(Id);      
        }

        public async Task<bool> RemoveRefreshTokenById(string tokenId)
        {
            var repo = new AuthenticateModuleDAO();

            if(await repo.CheckTokenById(tokenId))    
            {   
                return await repo.DeleteRefreshTokenById(tokenId);    
            }

            return false;     
        }

        public void Dispose()
        {   
            
        }
    }
}   

