using FinBaseWebApp.Helpers;
using FinBaseWebApp.Models;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FinBaseWebApp.Repository
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var userId = context.Ticket.Identity.Name;

            if(string.IsNullOrWhiteSpace(userId))
            {
                return;     
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            using(AuthenticationRepository _repo = new AuthenticationRepository())
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>("ta:clientRefreshTokenLifeTime");

                var token = new RefreshTokenModel()
                {
                    TOKENID = HashHelper.GetHash(refreshTokenId),
                    USERNAME = context.Ticket.Identity.Name,
                    ISSUEDDATETIME = DateTime.UtcNow,
                    EXPIREDDATETIME = DateTime.UtcNow.AddMinutes(30)
                };

                context.Ticket.Properties.IssuedUtc = token.ISSUEDDATETIME;         
                context.Ticket.Properties.ExpiresUtc = token.EXPIREDDATETIME;

                token.PROTECTEDTICKET = context.SerializeTicket();

                var result = await _repo.AddRefreshToken(token);

                if (result)
                {
                    context.SetToken(refreshTokenId);       
                }
            }
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = "*";
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            string hashedTokenId = HashHelper.GetHash(context.Token);
            using(AuthenticationRepository _repo = new AuthenticationRepository())
            {
                var refreshToken = await _repo.GetRefreshTokenById(hashedTokenId);

                if(refreshToken != null)
                {
                    context.DeserializeTicket(refreshToken.PROTECTEDTICKET);
                    var result = await _repo.RemoveRefreshTokenById(hashedTokenId);     
                }
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

    }
}

