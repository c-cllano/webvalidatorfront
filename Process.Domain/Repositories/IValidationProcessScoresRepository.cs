using static Domain.Enums.Enumerations;

namespace Process.Domain.Repositories
{
    public interface IValidationProcessScoresRepository
    {
        Task SaveValidationProcessScoresAsync(long validationProcessId, decimal scoreValue, ScoresCode scoreCode);
    }
}
