using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SampleJwtApp.Common;
using SampleJwtApp.Security.ViewModels;

namespace SampleJwtApp.Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public SecurityController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistration userRegistration)
        {
            var exists = await userManager.FindByNameAsync(userRegistration.Name);
            if (exists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            var appUser = new IdentityUser
            {
                UserName = userRegistration.Name,
                Email = userRegistration.Email,
                PhoneNumber = userRegistration.PhoneNo
            };

            var result = await userManager.CreateAsync(appUser, userRegistration.Password);
            if (!result.Succeeded)
            {
                // TODO: better messages for the obvious errors (password policy etc.)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            // TODO : Add other methods to manage roles
            //if (!await roleManager.RoleExistsAsync(userRegistration.UserRole))
            //{
            //    await roleManager.CreateAsync(new IdentityRole(userRegistration.UserRole));
            //}

            //if (await roleManager.RoleExistsAsync(userRegistration.UserRole))
            //{
            //    await userManager.AddToRoleAsync(appUser, userRegistration.UserRole);
            //}

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {
            var user = await userManager.FindByNameAsync(credentials.UserName);
            if (user == null || !await userManager.CheckPasswordAsync(user, credentials.Password))
            {
                return Unauthorized();
            }


            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                    new Claim(ClaimTypes.Email, user.Email),
                    // JWT specific values, Iss and Aud must be set to the same values as the server's
                    // otherwise the token will not be valid when used against the api
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, configuration["JsonWebTokenKeys:ValidIssuer"]),
                    new Claim(JwtRegisteredClaimNames.Aud, configuration["JsonWebTokenKeys:ValidAudience"]),
                };

            var userRoles = await userManager.GetRolesAsync(user);
            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JsonWebTokenKeys:SymmetricKey"]));

            var token = new JwtSecurityToken(
                // TODO : configurable token expiry ?
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                userName = user.UserName,
                status = "Login successful, token issued, send it back in a Bearer header to authenticate subsequent requests"
            });
        }
    }
}
