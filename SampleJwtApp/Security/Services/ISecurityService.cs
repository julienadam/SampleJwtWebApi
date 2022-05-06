using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SampleJwtApp.Security.Models;

namespace SampleJwtApp.Security.Services;

public interface ISecurityService
{
    Task<bool> UserExistsAsync(string userName);
    Task<IdentityResult?> AddUserAsync(string name, string email, string phoneNumber, string password);
    Task<LoggedInUser?> AuthenticateUserAsync(string username, string password);
    Task<SecurityToken> BuildJwtTokenAsync(LoggedInUser user);
    Task<bool> SendResetPasswordEmailLink(string email);
    Task<bool> ResetPassword(string userName, string token, string newPassword);
    Task<bool> EmailExistsAsync(string email);
}