namespace SampleJwtApp.Security.Services;

public interface IEmailSender
{
    Task<bool> SendEmailAsync(string to, string subject, string body);
}