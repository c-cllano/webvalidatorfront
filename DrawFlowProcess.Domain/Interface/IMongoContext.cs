using MongoDB.Driver;

namespace DrawFlowProcess.Domain.Interface
{
    public interface IMongoContext
    {
        IMongoDatabase GetDatabase();
    }
}
