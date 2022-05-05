namespace SampleJwtApp.Common.Email;

public class RetryingEmailSender<T> : IEmailSender
    where T : IEmailSender
{
    private readonly IEmailSender sender;
    private readonly int times;

    public RetryingEmailSender(IEmailSender sender, int times)
    {
        this.sender = sender;
        this.times = times;
    }

    public async Task<bool> SendEmail(string to, string subject, string body)
    {
        for (var i = 0; i < times; i++)
        {
            var result = await sender.SendEmail(to, subject, body);
            if(result)
            {
                return true;
            }
        }

        return false;
    }
}