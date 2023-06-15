
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using GatewayService.Models;
using GatewayService.Dto;
using GatewayService.Helpers;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GatewayService.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
         

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration            
        )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            _configuration = configuration;          
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] UserDto model)
        {
            var userExist = await userManager.FindByEmailAsync(model.Email);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "User already exist, kinldy login." });

            if (model.Password != model.ConfirmPassword)
                return StatusCode(StatusCodes.Status400BadRequest, new { status = false, message = "Password do not match" });


            //Create new user
            User user = new User()
                {
                    UserName = model.UserName,
                    Email = model.Email,

                    SecurityStamp = Guid.NewGuid().ToString(),
                    PhoneNumberConfirmed = true,

                    //LockoutEnabled = false,
                    //EmailConfirmed = true,
                    //TwoFactorEnabled = false,
                    //AccessFailedCount = 0
                    //PhoneNumber = model.PhoneNumber,

            };

                var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Failed to register new user", err = result.Errors.Select(e => e.Description) });
            }                   

                return Ok(new
                {
                    status = true,                    
                    message = "Sign up Successful"
                });
            
        }


        [HttpPost] 
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

                return Ok(new { status = false, message = messages });
            }


            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Email.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };             

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );                         

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
           
            return StatusCode(StatusCodes.Status401Unauthorized, new { status = false, message = "Invalid Username or Password." });
        }


        [Authorize]
        [HttpPut]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var currentUserId =  TokenClaims.GetLoggedInUserId(HttpContext); 

            var user = await userManager.FindByEmailAsync(currentUserId);

            if (user == null)
                return StatusCode(StatusCodes.Status404NotFound, new { status = false, message = "User does not exist." });

            if (string.Compare(model.NewPassword, model.ConfirmPassword) != 0)
                return StatusCode(StatusCodes.Status400BadRequest, new { status = false, message = "The new password and confirm new password does not match" });

            var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                var errors = new List<string>();

                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = string.Join(", ", errors) });
            }
            return Ok(new { status = true, message = "Password changed successfully" });
        }

        [Authorize]
        [HttpDelete]
        [Route("logout")]
        public async Task<IActionResult> Logout()       
        {             
            await signInManager.SignOutAsync();           

            return Ok(new { status = true, message = "User Logged out successfully" });
        }
    }
}

