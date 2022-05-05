using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;

namespace SampleJwtApp.Common.Email;

public class SendGridEmailSender : IEmailSender
{
    private readonly SendGridClient client;
    private readonly EmailAddress sender;

    public SendGridEmailSender(IConfiguration configuration)
    {
        client = new SendGridClient(configuration["Email.SendGrid.ApiKey"]);
        
        sender = new EmailAddress(configuration["Email.Sender"]);
    }

    public async Task<bool> SendEmail(string to, string subject, string body)
    {
        var msg = new SendGridMessage
        {
            From = sender,
            Subject = subject,
            Contents = new List<Content> { new PlainTextContent(body) }
        };
        msg.AddTo(to);

        var result = await client.SendEmailAsync(msg);

        return result.IsSuccessStatusCode;
    }
}