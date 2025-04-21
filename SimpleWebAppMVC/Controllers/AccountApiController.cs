using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NSwag.Annotations;
using SimpleWebAppMVC.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebAppMVC.Controllers
{
    [ApiController]
    [OpenApiTag("Account")]
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountApiController(IConfiguration cfg, SignInManager<IdentityUser> sm, UserManager<IdentityUser> um) : Controller
    {
        private readonly IConfiguration config = cfg;

        private readonly SignInManager<IdentityUser> signInManager = sm;
        private readonly UserManager<IdentityUser>   userManager   = um;

        private TokenModel GetToken(IdentityUser user)
        {
            var tokenLifetime = 60.0;
            var tokenSettings = this.config.GetRequiredSection("Token");
            var tokenKey      = tokenSettings.GetValue<string>("Key");
            var securityKey   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = tokenSettings.GetValue<string>("Audience"),
                Issuer   = tokenSettings.GetValue<string>("Issuer"),
                Expires  = DateTime.UtcNow.AddMinutes(tokenLifetime),
                Subject  = new ClaimsIdentity([ new Claim(ClaimTypes.Name, user.UserName) ]),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler  = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var response = new TokenModel
            {
                Token   = tokenHandler.WriteToken(securityToken),
                Expires = securityToken.ValidTo
            };

            return response;
        }

        /// <summary>Tries to authenticate the user, and returns a JWT access token if successful.</summary>
        /// <param name="authModel">Username and password</param>
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthModel authModel)
        {
            var user = await this.userManager.FindByEmailAsync(authModel.Email);

            if (user is not null)
            {
                var result = await this.signInManager.CheckPasswordSignInAsync(user, authModel.Password, false);

                if (result.Succeeded)
                    return Ok(GetToken(user));
            }

            ModelState.AddModelError("Password", "Invalid username or password.");

            return BadRequest(ModelState);
        }
    }
}
