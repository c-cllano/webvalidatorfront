using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IValidationProcessExecutionRepository
    {
        Task<ValidationProcessExecution?> GetValidationProcessExecutionById(long id);
        Task<ValidationProcessExecution?> GetValidationProcessExecutionByValidationProcessIdAsync(long validationProcessId);
        Task<ValidationProcessExecution> SaveValidationProcessExecutionAsync(ValidationProcessExecution validationProcessExecution);
        Task<ValidationProcessExecution> UpdateValidationProcessExecutionAsync(ValidationProcessExecution validationProcessExecution);
    }
}
