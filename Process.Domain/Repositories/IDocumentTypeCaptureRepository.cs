using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IDocumentTypeCaptureRepository
    {
        Task<DocumentTypeCapture?> GetByCodeOrDefaultAsync(string code);
    }
}