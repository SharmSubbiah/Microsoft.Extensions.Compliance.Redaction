namespace DataProtection;

public static partial class Logging
{
    [LoggerMessage(LogLevel.Information, "Customer Created")]
    public static partial void LogCustomerCreated(
        this ILogger logger,
        [LogProperties]Customer customer);
}