using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace SampleJwtApp.Security.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSender sender;

        public SecurityService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailSender sender)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.sender = sender ?? throw new ArgumentNullException(nameof(sender));
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

            return await userManager.CreateAsync(appUser, password);


            // TODO : Add other methods to manage roles
            //if (!await roleManager.RoleExistsAsync(userRegistration.UserRole))
            //{
            //    await roleManager.CreateAsync(new IdentityRole(userRegistration.UserRole));
            //}

            //if (await roleManager.RoleExistsAsync(userRegistration.UserRole))
            //{
            //    await userManager.AddToRoleAsync(appUser, userRegistration.UserRole);
            //}

        }

        public async Task<IdentityUser?> AuthenticateUserAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null && await userManager.CheckPasswordAsync(user, password))
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        
        public async Task<SecurityToken> BuildJwtTokenAsync(IdentityUser user)
        {
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

            return token;
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email)
        {
            var user = userManager.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                throw new ArgumentException("No user registered with that email address");
            }

            // Get a reset token
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            // NOTE: This is NOT how password reset should be handled IMHO, but this was specifically required.
            // NOTE: What should be done is send an email with a link containing the token and provide a web page to
            // NOTE: let the user set the new password, after validating the token.
            // NOTE: The obvious flaw here is that anyone with the email address of a person can force a change of password
            // NOTE: which denies the user in question access to the service...
            // NOTE: and the other obvious flaw is that the new password is sent in plain text in an email.
            // NOTE: We could force the user to pick a new password at next login though.

            // Generate a random password
            var pwd = new PasswordGenerator.Password(true, true, true, true, 12).Next();

            // Change the password using the token
            var result = await userManager.ResetPasswordAsync(user, token, pwd);
            if (result.Succeeded == false)
            {
                throw new Exception("Failed to reset password");
            }

            // Send an email with the newly modified password to the user
            return await sender.SendEmailAsync(email, "Password reset", "New password is : " + pwd);
        }
    }
}
