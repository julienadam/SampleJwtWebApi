namespace SampleJwtApp.Common.Email
{
    public class FallbackEmailSender<T1, T2> : IEmailSender 
        where T1 : IEmailSender 
        where T2 : IEmailSender
    {
        private readonly T1 sender1;
        private readonly T2 sender2;
        private readonly ILogger<FallbackEmailSender<T1, T2>> logger;

        public FallbackEmailSender(T1 sender1, T2 sender2, ILogger<FallbackEmailSender<T1, T2>> logger)
        {
            this.sender1 = sender1;
            this.sender2 = sender2;
            this.logger = logger;
        }
        public async Task<bool> SendEmail(string to, string subject, string body)
        {
            try
            {
                var result = await sender1.SendEmail(to, subject, body);
                if (result)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                logger.LogError($"Could not send email with {typeof(T1)}, trying {typeof(T2)}");
            }

            return await sender2.SendEmail(to, subject, body);
        }
    }
}
