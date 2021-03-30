using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using bookcaseApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace bookcaseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountsController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IConfiguration configuration)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
        }


        [HttpPost("Create")]
        public async Task<ActionResult<UserToken>> CreateUser ([FromBody] UserInfo userInfo)
        {
            var user = new ApplicationUser() { UserName = userInfo.Email, Email = userInfo.Email };
            var result = await _userManager.CreateAsync(user, userInfo.Password);
            if (result.Succeeded)
                return BuildToken(userInfo);
            else
                return BadRequest("User name o password invalid");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> LoginUser ([FromBody] UserInfo userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, false, false);
            if (result.Succeeded)
                return BuildToken(userInfo);
            else
                return BadRequest("User name o password invalid");
        }

        private UserToken BuildToken(UserInfo userInfo)
        {
            // Agregamos los valores que compondran nuestro token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("MyValue", "Aqui se pone lo que yo diga XD"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Creamos la key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            // Creamos las credenciales apartir de key.
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiracion del token.
            var expiration = DateTime.UtcNow.AddHours(2);

            // Creamos el token
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims, 
                expires: expiration,
                signingCredentials: cred);
           
            return new UserToken
            {
                Expiration = expiration,
                Token = new JwtSecurityTokenHandler().WriteToken(token) // convertimos el token en un string.
            };
        }
    }
}