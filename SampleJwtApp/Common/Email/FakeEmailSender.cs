namespace SampleJwtApp.Common.Email;

public class FakeEmailSender : IEmailSender
{
    private readonly ILogger<FakeEmailSender> logger;

    public FakeEmailSender(ILogger<FakeEmailSender> logger)
    {
        this.logger = logger;
    }

    public async Task<bool> SendEmail(string to,string subject, string body)
    {
        logger.LogInformation($"{to}\r\n{subject}\r\n{body}");
        return true;
    }
}