using MongoDB.Driver;

namespace DrawFlowConfiguration.Domain.Interfaces
{
 
    public interface IMongoContext
    {
        IMongoDatabase GetDataBase();
    }
}
