using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SampleJwtApp.Common;
using SampleJwtApp.Security.Services;
using SampleJwtApp.Security.ViewModels;

namespace SampleJwtApp.Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISecurityService securityService;

        public SecurityController(ISecurityService securityService)
        {
            this.securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistration userRegistration)
        {
            // TODO : more validation is required
            if (string.IsNullOrEmpty(userRegistration?.Password) || string.IsNullOrEmpty(userRegistration?.Name) || string.IsNullOrEmpty(userRegistration?.Email))
            {
                return BadRequest(new Response { Status = "Error", Message = "Missing data" });
            }

            // TODO: decide if we want to give this information or not, potential security concerns ?
            if (await securityService.UserExistsAsync(userRegistration.Name))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            }

            var result = await securityService.AddUserAsync(
                userRegistration.Name,
                userRegistration.Email,
                userRegistration.PhoneNo ?? "",
                userRegistration.Password);

            if (result?.Succeeded == false)
            {
                // TODO: better messages for the obvious errors (password policy etc.)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] Credentials credentials)
        {
            // TODO : more validation is required
            if (string.IsNullOrEmpty(credentials?.Password) || string.IsNullOrEmpty(credentials?.UserName))
            {
                return BadRequest(new Response { Status = "Error", Message = "Missing credentials" });
            }

            var user = await securityService.AuthenticateUserAsync(credentials.UserName, credentials.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = await securityService.BuildJwtTokenAsync(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                userName = credentials.UserName,
                status = "Login successful, token issued, send it back in a Bearer header to authenticate subsequent requests"
            });
        }

        [HttpPost]
        [Route("RequestPasswordReset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequest passwordResetRequest)
        {
            //TODO: validate that the input is a valid email address ?

            if (await securityService.SendPasswordResetEmailAsync(passwordResetRequest.Email, Request.PathBase + "/front/password-reset"))
            {
                return Ok(new
                {
                    status = "Success", message = "An email has been sent to the address provided. Please follow the link enclosed to reset your password."
                });
            }
            else
            {
                return StatusCode(500, new
                {
                    status = "Password reset email failed to be sent. Please try again later."
                });
            }
        }
    }
}
