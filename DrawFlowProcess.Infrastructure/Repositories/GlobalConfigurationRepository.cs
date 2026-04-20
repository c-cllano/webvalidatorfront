using DrawFlowProcess.Domain.Domain;
using DrawFlowProcess.Domain.Interface;
using DrawFlowProcess.Domain.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace DrawFlowProcess.Infrastructure.Repositories
{
    public class GlobalConfigurationRepository : IGlobalConfigurationRepository
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public GlobalConfigurationRepository(IMongoContext context)
        {
            _collection = context.GetDatabase().GetCollection<BsonDocument>("GlobalConfigurations");
        }

        public async Task<List<GlobalConfiguration>> GetGlobalConfigurationsAsync(Guid agreementId, int workFlowId, DateTime createDateTask, string section)
        {
            List<GlobalConfiguration> configurations = new();

            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("AgreementID", agreementId.ToString("D").ToLower()),
                Builders<BsonDocument>.Filter.Eq("WorkFlowID", workFlowId));

            var document = await _collection.Find(filter).FirstOrDefaultAsync();

            if (document == null)
            {
                configurations.Add(new GlobalConfiguration { Page = "expired", Result = ValidateGlobalConfig("", "Hours", 1, createDateTask) });
                return configurations;
            }

            var data = document.GetValue("GlobalConfiguration", null)?.AsBsonDocument
               .GetValue("GlobalConfiguration", null)?.AsBsonDocument
           ?? null;

            if (data == null)
                data = document["GlobalConfiguration"].AsBsonDocument;

            var reocord = data["Parameters"]["LifeCycle"][section].AsBsonDocument;

            var unit = reocord.GetValue("unitValue", 0).AsInt32;
            var value = reocord.GetValue("value", "").AsString;

            bool result = ValidateGlobalConfig(section, value, unit, createDateTask);

            GlobalConfiguration configuration = new()
            {
                Page = reocord.GetValue("page", "").AsString,
                Result = result,
            };

            configurations.Add(configuration);

            return configurations;
        }

        public async Task<bool> SaveGlobalConfigurationAsync(JsonDocument globalConfiguration)
        {
            try
            {
                var json = JsonSerializer.Serialize(globalConfiguration);
                var bson = BsonDocument.Parse(json);
                await _collection.InsertOneAsync(bson);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateGlobalConfigurationAsync(Guid agreementId, int workFlowId, JsonDocument globalConfiguration)
        {
            try
            {
                var json = JsonSerializer.Serialize(globalConfiguration);
                var bson = BsonDocument.Parse(json);
                var filter = Builders<BsonDocument>.Filter.And(
                    Builders<BsonDocument>.Filter.Eq("AgreementID", agreementId.ToString("D").ToLower()),
                    Builders<BsonDocument>.Filter.Eq("WorkFlowID", workFlowId));
                var update = Builders<BsonDocument>.Update.Set("GlobalConfiguration", bson.GetValue("GlobalConfiguration"));
                var result = await _collection.UpdateOneAsync(filter, update);
                return result.ModifiedCount > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<JsonDocument> GetGlobalConfigurationByFlow(Guid agreementId, int workFlowId)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("AgreementID", agreementId.ToString("D").ToLower()),
                Builders<BsonDocument>.Filter.Eq("WorkFlowID", workFlowId));
            var document = await _collection.Find(filter).FirstOrDefaultAsync();
            if (document == null)
                return null;
            var json = document.ToJson();
            return JsonDocument.Parse(json);
        }

        private bool ValidateGlobalConfig(string type, string value, int unit, DateTime date)
        {            
            var now = DateTime.UtcNow.AddHours(-5);

            bool result = false;
            switch (type)
            {
                case "applicationlifespan":

                    if (value == "Minutes")
                        result = (now > date.AddMinutes(unit));
                    if (value == "Hours")
                        result = (now > date.AddHours(unit));
                    if (value == "Days")
                        result = (now > date.AddDays(unit));
                    break;
                default:
                    if (value == "Hours")
                        result = (now > date.AddHours(unit));
                    break;
            }

            return result;
        }
    }
}
