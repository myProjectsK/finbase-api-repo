using FinBaseWebApp.Models;
using FinBaseWebApp.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Cors;
using System.Security.Cryptography;     

namespace FinBaseWebApp.Controllers
{   

    public class AuthenticateController : ApiController
    {

        private readonly AuthenticationRepository _authRepo;
        private readonly string _jwtSecret;
        private readonly int _expiryMinutes;

        public AuthenticateController()
        {
            _authRepo = new AuthenticationRepository();     
            _jwtSecret = ConfigurationManager.AppSettings["JwtSecretKey"];
            _expiryMinutes = int.Parse(ConfigurationManager.AppSettings["JwtExpiryMinutes"]);
        }

        private string GetSecretKey() => _jwtSecret;
        private int GetExpiryMinutes() => _expiryMinutes;

        [HttpPost]
        [Route("api/auth/login")]   
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login([FromBody] LoginModel loginReq)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid login request");

                var user = await _authRepo.LoginUser(loginReq.UserId, loginReq.PasswordHash);
                if (user == null)
                    return Content(HttpStatusCode.Unauthorized, "Invalid credentials");

                var token = GenerateJwtToken(user);
                // Generate refresh token
                var refreshToken = GenerateRefreshToken();

                // Store refresh token in the database
                var refreshTokenModel = new RefreshTokenModel
                {
                    TOKENID = refreshToken,
                    USERNAME = user.UserId,
                    ISSUEDDATETIME = DateTime.UtcNow,
                    EXPIREDDATETIME = DateTime.UtcNow.AddDays(5)        
                };

                await _authRepo.AddRefreshToken(refreshTokenModel);     

                return Ok(new { Token = token,
                    RefreshToken = refreshToken,
                    UserId = user.UserId, 
                    Email = user.EmailId
                });   
            }
            catch (Exception ex)
            {
                var details = ex.Message + Environment.NewLine + ex.ToString() + Environment.NewLine + (ex.InnerException == null ? "" : ex.InnerException.ToString());
                throw;
            }
        }   

        private string GenerateJwtToken(LoginModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), 
                    new Claim(ClaimTypes.Email, user.EmailId), 
                    new Claim(ClaimTypes.Role, user.RoleName)   
                }),     
                Expires = DateTime.UtcNow.AddMinutes(_expiryMinutes),   
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        [HttpPost]
        [Route("api/auth/refreshToken")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.RefreshToken))
                    return BadRequest("Refresh token is required");

                // Validate refresh token
                var storedToken = await _authRepo.GetRefreshTokenById(request.RefreshToken);
                if (storedToken == null)
                    return Content(HttpStatusCode.Unauthorized, "Invalid refresh token");

                if (storedToken.EXPIREDDATETIME < DateTime.UtcNow)
                {
                    // Delete expired token
                    await _authRepo.RemoveRefreshTokenById(storedToken.TOKENID);
                    return Content(HttpStatusCode.Unauthorized, "Refresh token expired");
                }

                // Get user information
                var user = await _authRepo.GetUserByUsername(storedToken.USERNAME);     
                if (user == null)
                    return Content(HttpStatusCode.Unauthorized, "User not found");

                // Generate new access token
                var newAccessToken = GenerateJwtToken(user);
                // Generate new refresh token (token rotation for better security)
                var newRefreshToken = GenerateRefreshToken();
                // Delete old refresh token
                await _authRepo.RemoveRefreshTokenById(storedToken.TOKENID);

                // Store new refresh token
                var refreshTokenModel = new RefreshTokenModel
                {
                    TOKENID = newRefreshToken,
                    USERNAME = user.UserId,
                    ISSUEDDATETIME = DateTime.UtcNow,
                    EXPIREDDATETIME = DateTime.UtcNow.AddDays(5)
                };

                await _authRepo.AddRefreshToken(refreshTokenModel);

                // Return new tokens
                return Ok(new
                {
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                var details = ex.Message + Environment.NewLine + ex.ToString() + Environment.NewLine + (ex.InnerException == null ? "" : ex.InnerException.ToString());
                throw;
            }
        }

        public class RefreshTokenRequest
        {
            public string RefreshToken { get; set; }
        }
    }
}
