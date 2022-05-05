namespace SampleJwtApp.Common.Email
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(string to, string subject, string body);
    }
}
