using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NFluent;
using SampleJwtApp.Security.Controllers;
using SampleJwtApp.Security.Services;
using SampleJwtApp.Security.ViewModels;
using Xunit;

namespace SampleJwtWebApi.Tests
{
    public class SecurityControllerTests
    {
        private class NopeSecurityService : ISecurityService
        {
            public Task<bool> UserExistsAsync(string userName)
            {
                throw new System.NotImplementedException();
            }

            public Task<IdentityResult?> AddUserAsync(string name, string email, string phoneNumber, string password)
            {
                throw new System.NotImplementedException();
            }

            public Task<LoggedInUser?> AuthenticateUserAsync(string username, string password)
            {
                throw new System.NotImplementedException();
            }

            public Task<SecurityToken> BuildJwtTokenAsync(LoggedInUser user)
            {
                throw new System.NotImplementedException();
            }

            public Task<bool> SendResetPasswordEmailLink(string email)
            {
                throw new System.NotImplementedException();
            }

            public Task<bool> ResetPassword(string userName, string token, string newPassword)
            {
                throw new System.NotImplementedException();
            }

            public Task<bool> EmailExistsAsync(string email)
            {
                throw new System.NotImplementedException();
            }
        }

        [Fact]
        public async Task Logging_in_without_a_username_fails()
        {
            var controller = new SecurityController(new NopeSecurityService());
            var result = await controller.Login(new Credentials());
            Check.That(result).IsInstanceOf<BadRequestObjectResult>();
        }
    }
}