namespace WebApi.Extensions
{
    public static class LoggerExtensions
    {
        public static void LogInformationWithUtc(
            this ILogger logger,
            string message)
        {
            logger.LogInformation(
                "[{UtcTime:HH:mm:ss.fff}] {Message}",
                DateTime.UtcNow,
                message);
        }
    }
}
