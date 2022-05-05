using System.Net;
using System.Net.Mail;

namespace SampleJwtApp.Common.Email;

public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpClient smtpClient;
    private readonly string sender;

    public SmtpEmailSender(IConfiguration config)
    {
        smtpClient = new SmtpClient();
        smtpClient.Host = config["Email.Host"] ?? "localhost";
        smtpClient.Port = int.Parse(config["Email.Port"] ?? "25");
        smtpClient.EnableSsl = bool.Parse(config["Email.UseSsl"] ?? "false");
        smtpClient.Credentials = new NetworkCredential(config["Email.UserName"] ?? "", config["Email.Password"] ?? "");
        sender = config["Email.Sender"] ?? "no-reply@example.com";
    }

    public async Task<bool> SendEmail(string to, string subject, string body)
    {
        smtpClient.Send(sender, to, subject, body);
        return true;
    }
}