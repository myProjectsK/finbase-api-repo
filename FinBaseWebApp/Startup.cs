using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System.Text;
using System.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Web.Http;
using FinBaseWebApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(FinBaseWebApp.Startup))] 

public partial class Startup
{
    public void Configuration(IAppBuilder app)
    {
        ConfigureAuth(app);     

        HttpConfiguration config = new HttpConfiguration();
        WebApiConfig.Register(config);
        app.UseWebApi(config);
    }

    public void ConfigureAuth(IAppBuilder app)
    {
        app.UseCors(CorsOptions.AllowAll);      
        var secretKey = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["JwtSecretKey"]); 

        app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
        {
            AuthenticationMode = AuthenticationMode.Active,
            TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey), 
                ValidateIssuer = false, 
                ValidateAudience = false, 
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }
        });     
    }

}

