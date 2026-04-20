
namespace Process.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {

        int Commit();

    }
}
