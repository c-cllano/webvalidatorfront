using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface IUserOkeyRepository
    {
        Task<long> AddAsync(UserOkey user);
        Task<UserOkey?> GetByUserIdAsync(long userId);
        Task<List<UserOkey>> GetUsers(IEnumerable<int>? usersId);
        Task UpdateAsync(UserOkey user);
    }
}
