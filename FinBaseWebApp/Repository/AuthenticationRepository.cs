using FinBaseWebApp.Models;
using FinBaseWebApp.Readers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinBaseWebApp.Repository
{
    public class AuthenticationRepository : IDisposable
    {
        private readonly AuthenticateModuleDAO _authDAO;    
        private readonly string _connectionString;

        public AuthenticationRepository()
        {
            _authDAO = new AuthenticateModuleDAO();     
        }

        public async Task<LoginModel> LoginUser(string UserName, string Password)
        {
            return await _authDAO.Public_LoginUser(UserName, Password);
        }

        public async Task<LoginModel> GetUserByUsername(string UserId)
        {
            return await _authDAO.GetUserDAO(UserId);
        }

        /*public async Task<List<RefreshTokenModel>> GetAllRefreshToken()
        {
            var repo = new AuthenticateModuleDAO();
            return await repo.GetAllTokens();     
        }*/

        public async Task<RefreshTokenModel> GetRefreshTokenById(string tokenId)
        {
            //var repo = new AuthenticateModuleDAO();
            return await _authDAO.GetTokenById(tokenId);     
        }

        public async Task<bool> AddRefreshToken(RefreshTokenModel refreshToken)
        {
            //var repo = new AuthenticateModuleDAO();
            if (await _authDAO.CheckTokenByUserName(refreshToken))    
            {   
                var result = await _authDAO.DeleteRefreshTokenById(refreshToken.TOKENID);     
            }

            var Id = await _authDAO.InsertRefreshToken(refreshToken);
            return !string.IsNullOrWhiteSpace(Id);      
        }

        public async Task<bool> RemoveRefreshTokenById(string tokenId)
        {
            //var repo = new AuthenticateModuleDAO();
            if(await _authDAO.CheckTokenById(tokenId))    
            {   
                return await _authDAO.DeleteRefreshTokenById(tokenId);    
            }

             return false;     
        }

        public void Dispose()
        {   
            
        }
    }
}   

