using Process.Domain.Entities;
using Process.Domain.Services;
using System.Threading.Channels;

namespace Process.Infrastructure.Services
{
    public class AuditQueueService : IAuditQueueService
    {
        private readonly Channel<ValidationProcessAuditLogs> _queue;

        public AuditQueueService()
        {
            var options = new BoundedChannelOptions(1000) { FullMode = BoundedChannelFullMode.Wait };
            _queue = Channel.CreateBounded<ValidationProcessAuditLogs>(options);
        }

        public async ValueTask QueueAuditLog(ValidationProcessAuditLogs log) => await _queue.Writer.WriteAsync(log);

        public IAsyncEnumerable<ValidationProcessAuditLogs> DequeueAllAsync(CancellationToken ct) => _queue.Reader.ReadAllAsync(ct);
    }
}
