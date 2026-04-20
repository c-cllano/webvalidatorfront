using Process.Domain.Parameters.ExternalApiClientParameters;

namespace Process.Domain.Services
{
    public interface IJumioService
    {
        Task<TokenJumioResponse> GetTokenJumioAsync();
        Task<ClientAccountJumioResponse> CreateClientJumioAsync(string token);
        Task ExecuteUrlsFrontBackAsync(string token, string url, string base64);
        Task<Guid> ExecuteUrlFinalizedProcessAsync(string token, string url);
        Task ExecuteDeleteUserInfoOnJumio(string token, Guid workflowExecutionId);
        Task<GetProcessJumioResponse> GetResultJumioProcessAsync(string token, Guid workflowExecutionId);
    }
}
