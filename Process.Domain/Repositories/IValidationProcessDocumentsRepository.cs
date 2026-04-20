using Process.Domain.Entities;
using Process.Domain.Parameters.ProcessDocuments;

namespace Process.Domain.Repositories
{
    public interface IValidationProcessDocumentsRepository
    {
        Task SaveValidationProcessDocumentsAsync(long validationProcessId, ProcessDocumentsRequest processDocumentsRequest);
        Task<List<ValidationProcessDocuments>> GetDocumentsByValidationProcessIdAsync(long validationProcessId);
    }
}
