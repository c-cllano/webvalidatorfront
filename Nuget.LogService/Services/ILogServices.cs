using Nuget.LogService.Models;

namespace Nuget.LogService.Services
{
    public interface ILogServices
    {
        Task<bool> CreateErrorAsync(CreateErrorIn error);
        Task<bool> CreateRequestAsync(CreateRequest request);
    }
}
