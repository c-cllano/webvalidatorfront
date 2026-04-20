using Process.Domain.Parameters.Atdpt;
using Process.Domain.ValueObjects;

namespace Process.Application.Interfaces
{
    public interface IAtdpApiClient
    {
        Task<AtdpVersionFile> GetFileVersionByIDAsync(int atdpID, string tokenAdtp);
        Task<AtdpTransactionFile> GetFileTransactionByIdAsync(int atdpTransactionID, string tokenAdtp);
        Task<AtdpTransactionSave> SaveTransactionAsync(SaveTransactionRequest request, string tokenAdtp);
    }
}
