using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IValidationProcessAuditLogsRepository
    {
        Task SaveAuditLogsAsync(ValidationProcessAuditLogs auditLogs);
    }
}
