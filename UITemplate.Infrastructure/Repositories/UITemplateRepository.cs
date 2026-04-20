using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using UITemplate.Domain.Interface;
using UITemplate.Domain.Repositories;

namespace UITemplate.Infrastructure.Repositories
{
    public class UITemplateRepository : IUITemplateRepository
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public UITemplateRepository(IMongoContext context)
        {
            _collection = context.GetDataBase().GetCollection<BsonDocument>("Template");
        }

        public async Task<object> GetUItemplate(string clientId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.guid", clientId);
            var projection = Builders<BsonDocument>.Projection.Include("UIConfigurationJson");
            var document = await _collection.Find(filter).Project(projection).FirstOrDefaultAsync();
            if (document != null)
                return BsonSerializer.Deserialize<object>(document);

            return null;
        }
    }
}
