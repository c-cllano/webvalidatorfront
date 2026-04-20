using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Process.Domain.Repositories;
using Process.Domain.Services;
using Process.Domain.Utilities;
using System.Buffers;
using System.IO.Pipelines;
using System.Text.Json;

namespace Process.Infrastructure.Services
{
    public class AuditWorkerService(
        IAuditQueueService queue,
        IServiceProvider serviceProvider,
        ILogger<AuditWorkerService> logger
    ) : BackgroundService
    {
        private readonly IAuditQueueService _queue = queue;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly ILogger<AuditWorkerService> _logger = logger;

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken
        )
        {
            await foreach (var log in _queue.DequeueAllAsync(stoppingToken))
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var _blobStorageService = scope.ServiceProvider.GetRequiredService<IBlobStorageService>();
                    var _auditLogsRepository = scope.ServiceProvider.GetRequiredService<IValidationProcessAuditLogsRepository>();

                    log.ValidationProcessId = await ExtractValidationProcessId(log.RequestBody!, stoppingToken)
                        ?? AuditJsonHelper.ExtractValidationProcessId(log.ResponseBody);

                    if (File.Exists(log.RequestBody))
                    {
                        string filePath = log.RequestBody;

                        log.RequestBody = await _blobStorageService.UploadFilePathAsync(log.RequestBody);

                        File.Delete(filePath);
                    }

                    await _auditLogsRepository.SaveAuditLogsAsync(log);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error procesando auditoría diferida");
                }
            }
        }

        private static async Task<long?> ExtractValidationProcessId(
            string filePath,
            CancellationToken ct
        )
        {
            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
                return null;

            await using var fs = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 8192,
                useAsync: true
            );

            var pipe = PipeReader.Create(fs);
            var state = new JsonReaderState();

            try
            {
                while (true)
                {
                    var result = await pipe.ReadAsync(ct);
                    var buffer = result.Buffer;

                    if (TryReadValidationProcessId(ref buffer, result.IsCompleted, ref state, out var id))
                        return id;

                    pipe.AdvanceTo(buffer.Start, buffer.End);

                    if (result.IsCompleted)
                        break;
                }
            }
            finally
            {
                await pipe.CompleteAsync();
            }

            return null;
        }

        private static bool TryReadValidationProcessId(
            ref ReadOnlySequence<byte> buffer,
            bool isFinalBlock,
            ref JsonReaderState state,
            out long id
        )
        {
            var reader = new Utf8JsonReader(buffer, isFinalBlock, state);

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName &&
                    (reader.ValueTextEquals("validationProcessId") || reader.ValueTextEquals("ValidationProcessId")) &&
                    reader.Read() &&
                    reader.TokenType == JsonTokenType.Number)
                {
                    id = reader.GetInt64();
                    buffer = buffer.Slice(reader.Position);
                    state = reader.CurrentState;
                    return true;
                }
            }

            buffer = buffer.Slice(reader.Position);
            state = reader.CurrentState;

            id = default;
            return false;
        }
    }
}
