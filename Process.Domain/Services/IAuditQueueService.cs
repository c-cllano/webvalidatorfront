using Process.Domain.Entities;

namespace Process.Domain.Services
{
    public interface IAuditQueueService
    {
        ValueTask QueueAuditLog(ValidationProcessAuditLogs log);
        IAsyncEnumerable<ValidationProcessAuditLogs> DequeueAllAsync(CancellationToken ct);
    }
}
