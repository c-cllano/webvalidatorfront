using Serilog;

namespace Nuget.LogService.Services
{
    public static class BlobLoggerService
    {
        public static ILogger CreateBlobLogger(
            string connectionString,
            string containerName,
            string processExecuteLog
        )
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.AzureBlobStorage(
                    connectionString: connectionString,
                    storageContainerName: containerName,
                    storageFileName: $"{processExecuteLog}/log-{DateTime.Now:yyyyMMdd}.txt"
                )
                .CreateLogger();
        }
    }
}
