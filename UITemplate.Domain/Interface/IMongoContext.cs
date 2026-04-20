using MongoDB.Driver;

namespace UITemplate.Domain.Interface
{
    public interface IMongoContext
    {
        IMongoDatabase GetDataBase();
    }
}
