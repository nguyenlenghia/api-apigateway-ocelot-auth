using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiGatewayOcelot.Service1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AccountController()
        {
        }

        /// <summary>
        /// Xác thực user login và tạo token xác thực
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>Jwt token xác thực người dùng đăng nhập thành công</returns>
        [AllowAnonymous]
        [HttpGet("Login")]
        public ActionResult<string> Login(string username, string password)
        {
            if (username != password)
            {
                return BadRequest();
            }

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, username + "xxx"),
                new Claim("Name", username),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "App", "Name", "Role");
            

            // authentication successful so generate jwt token
            var strToken = CreateToken(claimsIdentity, out JwtSecurityToken token);

            return Ok(strToken);
        }

        [Authorize]
        [HttpGet("TestUserName")]
        public ActionResult<string> TestUserName()
        {
            return Ok($"user: {this.User.Identity.Name}");
        }

        private string CreateToken(ClaimsIdentity claims, out JwtSecurityToken token)
        {
            var now = DateTime.Now;
            var expires = now.AddDays(30);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myAppoihekjw98232hk"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            token = new JwtSecurityToken(

                issuer: "yourdomain.com",
                audience: "service1",
                claims: claims.Claims,
                expires: expires,
                notBefore: now,
                signingCredentials: creds
            );
            var strToken = tokenHandler.WriteToken(token);
            return strToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myAppoihekjw98232hk")),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}