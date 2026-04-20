namespace Process.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Entities.UserInfo?> GetUserById(long id);
    }
}
