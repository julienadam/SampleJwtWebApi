namespace SampleJwtApp.Security.Services
{
    public class LogMailSender : IEmailSender
    {
        private readonly ILogger<LogMailSender> logger;

        public LogMailSender(ILogger<LogMailSender> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            logger.LogInformation(
                $"To: {to}\r\nSubject:{subject}\r\nBody:\r\n{body}");

            return true;
        }
    }
}
