using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using Dapper;
using FinBaseWebApp.Helpers;
using FinBaseWebApp.Repository;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;    

namespace FinBaseWebApp
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]      

    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider     
    {

        private readonly PublicUserRepository _publicRepo;
        private readonly AuthenticateHelper _authHelp;      

        public AuthorizationServerProvider()
        {
            _publicRepo = new PublicUserRepository();
            _authHelp = new AuthenticateHelper();   
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();    
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var Identity = new ClaimsIdentity(context.Ticket.Identity);     
            Identity.AddClaim(new Claim("newClaim", "newValue"));
            var newTicket = new AuthenticationTicket(Identity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);     
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["FinBaseDB"].ConnectionString;

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            try
            {
/*                bool queryParam = false;

                if(string.IsNullOrEmpty(context.UserName))
                {
                    context.SetError("Invalid grant", "Username cannot be null or empty");
                    return;
                }

                if (_authHelp == null)
                {
                    context.SetError("Invalid grant", "_authHelp instance is null");
                    return;
                }

                if (double.TryParse(context.UserName, out _) || _authHelp.IsEmail(context.UserName))
                {
                    queryParam = true;
                }
*/
                if (double.TryParse(context.UserName, out _) || _authHelp.IsEmail(context.UserName))
                {
                    var user = await _publicRepo.LoginUser(context.UserName, context.Password);

                    if(user != null)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("Invalid grant", "Provided credentials are incorrect");
                        context.Rejected();     
                    }
                }   
                return;     
            }
            catch (Exception ex)
            {
                context.SetError("Invalid_Grant", ex.ToString() + ex.Message + ex.InnerException.ToString());
                context.Rejected();     
            }
        }
    }
}

