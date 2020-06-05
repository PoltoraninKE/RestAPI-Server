using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MYRestApiServer.Core.Models;
using MYRestApiServer.Core.Options;

namespace MYRestApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly Core.Models.AppContext appContext;
        private readonly IConfiguration configuration;

        public AccountController(IConfiguration _configuration, Core.Models.AppContext _appContext) 
        {
            configuration = _configuration;
            appContext = _appContext;
        }

        [HttpPost("/token")]
        public IActionResult GetToken() 
        {
            var encoded = AuthenticationHeaderValue.Parse(Request.Headers[HeaderNames.Authorization]).Parameter;
            var data = Convert.FromBase64String(encoded);
            var authParams = Encoding.UTF8.GetString(data).Split(":");

            var identity = GetIdentity(authParams[0], authParams[1]);
            if (identity == null)
            {
                return Forbid();
            }
            //BadRequest(new { errorText = "Invalid username or password." })
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: configuration.GetValue<string>("AuthOptions:issuer"),
                    audience: configuration.GetValue<string>("AuthOptions:audience"),
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(configuration.GetValue<int>("AuthOptions:lifetime"))),
                    signingCredentials: new SigningCredentials(KeyGen.GetSymmetricSecurityKey(configuration.GetValue<string>("AuthOptions:key")), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return Ok(response);
        }

        private ClaimsIdentity GetIdentity(string login, string password)
        {
            var user = appContext.Users.FirstOrDefault(x => x.Login == login && x.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                };

                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }

        [HttpPost("/sign_in")]
        public async Task<IActionResult> PostUser([FromBody] User User)
        {
            await appContext.Users.AddAsync(User);
            await appContext.SaveChangesAsync();
            return Ok();
        }

    }
}