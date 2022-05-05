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
        smtpClient.Host = config["Email.Host"];
        smtpClient.Port = int.Parse(config["Email.Port"]);
        smtpClient.EnableSsl = bool.Parse(config["Email.UseSsl"]);
        smtpClient.Credentials = new NetworkCredential(config["Email.UserName"], config["Email.Password"]);
        sender = config["Email.Sender"];
    }

    public async Task<bool> SendEmail(string to, string subject, string body)
    {
        smtpClient.Send(sender, to, subject, body);
        return true;
    }
}