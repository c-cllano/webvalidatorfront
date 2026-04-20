using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DrawFlowConfiguration.Domain.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

using MongoDB.Bson.Serialization;
using DrawFlowConfiguration.Domain.Interfaces;
using DrawFlowConfiguration.Application.DrawFlowConfiguration.PostJsonWorkflows;


namespace DrawFlowConfiguration.Infrastructure.Repositories
{

    public class JsonWorkflowRepository : IJsonWorkflowRepository

    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public JsonWorkflowRepository(IMongoContext context)
        {
            _collection = context.GetDataBase().GetCollection<BsonDocument>("WorkFlow");
        }




        public async Task<object> GetUIConfiguration(Guid? agreementId = null, int? workFlowId = null)
        {
            var filters = new List<FilterDefinition<BsonDocument>>();

            if (agreementId.HasValue)
            {
                filters.Add(Builders<BsonDocument>.Filter.Eq("AgreementID", agreementId.Value.ToString("D")));
            }

            if (workFlowId.HasValue)
            {
                filters.Add(Builders<BsonDocument>.Filter.Eq("WorkFlowID", workFlowId.Value));
            }

            var filter = filters.Count > 0
                ? Builders<BsonDocument>.Filter.And(filters)
                : Builders<BsonDocument>.Filter.Empty;

            var documents = await _collection.Find(filter).ToListAsync();

            return documents.Select(doc => BsonSerializer.Deserialize<object>(doc)).ToList();
        }







        public async Task<object> SaveUIConfiguration(object jsonConfiguration)
        {
            var json = JsonSerializer.Serialize(jsonConfiguration);
            var bson = BsonDocument.Parse(json);

            await _collection.InsertOneAsync(bson);


            var insertedId = bson.GetValue("_id");
            var insertedDoc = await _collection.Find(Builders<BsonDocument>.Filter.Eq("_id", insertedId)).FirstOrDefaultAsync();

            if (insertedDoc != null)
            {
                Console.WriteLine("Documento insertado:");
                Console.WriteLine(insertedDoc.ToJson());
            }
            else
            {
                Console.WriteLine("No se encontró el documento insertado.");
            }

            return BsonSerializer.Deserialize<object>(bson);
        }


        public async Task<bool> UpdateUIConfiguration(Guid agreementId, int workFlowId, object jsonConfiguration)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("AgreementID", agreementId.ToString("D")),
                Builders<BsonDocument>.Filter.Eq("WorkFlowID", workFlowId)
            );

            var jsonElement = (JsonElement)jsonConfiguration;
            var rawJson = jsonElement.GetRawText();
            var bsonDoc = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(rawJson);

            var update = Builders<BsonDocument>.Update
                .Set("drawflow", bsonDoc);

            var result = await _collection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;
        }



        public async Task DeleteIUConfiguration(string clientId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.guid", clientId);
            await _collection.DeleteOneAsync(filter);
            return;
        }


        public async Task<bool> DeleteWorkflow(Guid agreementId, int workFlowId)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("AgreementID", agreementId.ToString("D")),
                Builders<BsonDocument>.Filter.Eq("WorkFlowID", workFlowId)
            );

            var result = await _collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0;
        }
    }
}
