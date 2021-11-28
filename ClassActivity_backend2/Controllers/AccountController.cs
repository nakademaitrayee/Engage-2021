using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ClassActivity_backend2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        readonly UserManager<IdentityUser> userManager;
        readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] Models.Credentials cred)
        {
            var user = new IdentityUser
            {
                UserName = cred.Email,
                Email = cred.Email
            };

            var credPassed = await userManager.CreateAsync(user, cred.Password);

            if(!credPassed.Succeeded)
            {
                return BadRequest(credPassed.Errors);
            }

            await signInManager.SignInAsync(user, isPersistent: false);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the server key used to sign the JWT token is here"));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(signingCredentials: signingCredentials,claims:claims);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { tkn = tokenString });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.Credentials cred)
        {
            var isPassed = await signInManager.PasswordSignInAsync(cred.Email, cred.Password,false,false);

            if (!isPassed.Succeeded)
                return BadRequest(new { error = "error occurred"});

            var user = await userManager.FindByEmailAsync(cred.Email);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the server key used to sign the JWT token is here"));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { tkn = tokenString });
        }
    }
}