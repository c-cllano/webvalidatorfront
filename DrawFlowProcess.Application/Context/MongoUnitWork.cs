using DrawFlowProcess.Domain.Interface;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DrawFlowProcess.Application.Context
{
    public class MongoUnitWork : IMongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoUnitWork(IConfiguration config)
        {
            var connectionstring = config["ConnectionStrings:MongoDb"];
            var dataBaseName = config["MongoDbSettings:Database"];

            var client = new MongoClient(connectionstring);
            _database = client.GetDatabase(dataBaseName);
        }

        public IMongoDatabase GetDatabase() => _database;
    }
}
