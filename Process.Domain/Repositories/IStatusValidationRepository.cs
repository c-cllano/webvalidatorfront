using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IStatusValidationRepository
    {
        Task<StatusValidation?> GetStatusValidationByStatusCodeAsync(int statusCode);
    }
}
