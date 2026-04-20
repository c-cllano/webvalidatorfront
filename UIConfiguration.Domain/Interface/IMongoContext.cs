using MongoDB.Driver;

namespace UIConfiguration.Domain.Interface
{
    public interface IMongoContext
    {
        IMongoDatabase GetDataBase();
    }
}
