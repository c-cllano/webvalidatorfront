using Process.Domain.Entities;
using Process.Domain.Parameters.Sso.Authentications;
using Process.Domain.Parameters.Sso.User;

namespace Process.Domain.Services
{
    public interface ISsoService
    {
        Task<SsoServiceResult<TokenResponse>> GetToken(Token getToken);
        Task<int> ForgotPassword(ForgotPassword forgotPassword);
        Task<object?> ResetPassword(ResetPassword resetPassword);
        Task<object?> ChangePassword(ChangePassword changePassword);
        Task<object?> CreateClient(CreateClient createClient);
        Task<object?> ApplicationCreate(ApplicationCreate applicationCreate);
        Task<UserSsoCreated?> Register(CreateUser createUser);
        Task<UserSso> GetUser(int userId);
        Task<SsoServiceResult<List<UserSso>>> GetUsers(UserQueryParameters user);
        Task<object?> UpdateUser(int userId, UpdateUser updateUser);
        Task<object?> DeleteUser(int userId);
    }
}
