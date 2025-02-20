using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using FinBaseWebApp.Repository;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(FinBaseWebApp.Startup))]

namespace FinBaseWebApp
{
    public partial class Startup
    {

        public void ConfigurationAuth(IAppBuilder app)
        {
     
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(300),      
                Provider = new AuthorizationServerProvider(),       
                RefreshTokenProvider = new RefreshTokenProvider()
            };

            app.UseOAuthAuthorizationServer(options);       
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);      
        }   

        public void Configuration(IAppBuilder app)
        {
            ConfigurationAuth(app);     
        }
    }
}

