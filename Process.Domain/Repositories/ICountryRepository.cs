using Process.Domain.Entities;

namespace Process.Domain.Repositories
{
    public interface ICountryRepository
    {
        Task<Country?> GetCountryById(int id);
    }
}
