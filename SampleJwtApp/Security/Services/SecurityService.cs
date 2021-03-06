using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using SampleJwtApp.Common;
using SampleJwtApp.Common.DataAccess;
using SampleJwtApp.Common.Email;
using SampleJwtApp.Security.Models;
using SampleJwtApp.UserPrefs.Entities;

namespace SampleJwtApp.Security.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly AppDbContext context;
        private readonly IEmailSender sender;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private string? frontUrl;

        public SecurityService(AppDbContext context, IEmailSender sender, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.context = context;
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await userManager.FindByEmailAsync(email) != null;
        }

        public async Task<bool> UserExistsAsync(string userName)
        {
            return await userManager.FindByNameAsync(userName) != null;
        }
        
        public async Task<IdentityResult?> AddUserAsync(string name, string email, string phoneNumber, string password)
        {
            var appUser = new IdentityUser
            {
                UserName = name,
                Email = email,
                PhoneNumber = phoneNumber
            };


            var userPrefs = new UserPreferences
            {
                Balance = 42.09,
                User = appUser
            };


            var isFirstUser = !userManager.Users.Any();
            var user = await userManager.CreateAsync(appUser, password);
            context.Preferences.Add(userPrefs);
            await context.SaveChangesAsync();

            if (isFirstUser)
            {
                // If it is the first user, grant the admin role
                if (!await roleManager.RoleExistsAsync(RoleDefinitions.AdministratorRoleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(RoleDefinitions.AdministratorRoleName));
                }
                await userManager.AddToRoleAsync(appUser, RoleDefinitions.AdministratorRoleName);
            }

            return user;
        }

        public async Task<LoggedInUser?> AuthenticateUserAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null && await userManager.CheckPasswordAsync(user, password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                return new LoggedInUser(user, userRoles);
            }
            else
            {
                return null;
            }
        }
        
        public async Task<SecurityToken> BuildJwtTokenAsync(LoggedInUser loggedInUser)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loggedInUser.User.UserName),
                    new Claim(ClaimTypes.MobilePhone, loggedInUser.User.PhoneNumber),
                    new Claim(ClaimTypes.Email, loggedInUser.User.Email),
                    // JWT specific values, Iss and Aud must be set to the same values as the server's
                    // otherwise the token will not be valid when used against the api
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, configuration["JsonWebTokenKeys:ValidIssuer"]),
                    new Claim(JwtRegisteredClaimNames.Aud, configuration["JsonWebTokenKeys:ValidAudience"]),
                };

            authClaims.AddRange(loggedInUser.Roles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JsonWebTokenKeys:SymmetricKey"]));

            var token = new JwtSecurityToken(
                // TODO : configurable token expiry ?
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        public async Task<bool> SendResetPasswordEmailLink(string email)
        {
            var user = userManager.Users.First(u => u.Email == email);
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            
            frontUrl = configuration["Front:BaseUrl"];
            var url = $"{frontUrl}/reset-password?username={Uri.EscapeDataString(user.UserName)}&token={Uri.EscapeDataString(token)}";
            return  await sender.SendEmail(email, "Reset password", "Please confirm by clicking the following link.\r\n\r\n" + url);
        }

        public async Task<bool> ResetPassword(string userName, string token, string newPassword)
        {
            var user = userManager.Users.First(u => u.UserName == userName);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
    }
}
