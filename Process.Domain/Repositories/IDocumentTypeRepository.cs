using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IDocumentTypeRepository
    {
        Task<DocumentType?> GetDocumentTypeById(int documentTypeId);
        Task<DocumentType?> GetDocumentTypeByCode(string code);
    }
}
