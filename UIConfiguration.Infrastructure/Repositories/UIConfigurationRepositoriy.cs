using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using UIConfiguration.Domain.Interface;
using UIConfiguration.Domain.Repositories;

namespace UIConfiguration.Infrastructure.Repositories
{
    public class UIConfigurationRepositoriy : IUIConfigurationRepository
    {
        private readonly IMongoCollection<BsonDocument> _collection;

        public UIConfigurationRepositoriy(IMongoContext context)
        {
            _collection = context.GetDataBase().GetCollection<BsonDocument>("Configurations");
        }

        public async Task<object> GetUIConfiguration(string clientId, int workflowID)
        {

            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.guid", clientId),
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.workflowid", workflowID));
            var projection = Builders<BsonDocument>.Projection.Include("UIConfigurationJson");
            var document = await _collection.Find(filter).Project(projection).FirstOrDefaultAsync();
            if (document != null)
                return BsonSerializer.Deserialize<object>(document);

            return null!;
        }

        public async Task<object> SaveUIConfiguration(object jsonConfiguration)
        {
            var json = JsonSerializer.Serialize(jsonConfiguration);
            var bson = BsonDocument.Parse(json);
            await _collection.InsertOneAsync(bson);
            return BsonSerializer.Deserialize<object>(bson);
        }

        public async Task<object> UpdateIUConfiguration(object jsonConfiguration, string clientId, int workflowID)
        {
            var json = JsonSerializer.Serialize(jsonConfiguration);
            var bson = BsonDocument.Parse(json);

            if (!bson.Contains("agreement") || !bson["agreement"].AsBsonDocument.Contains("guid"))
                throw new ArgumentException("Se requiere el campo guid para actualizar.");

            if (bson["agreement"]["guid"].AsString != clientId)
                throw new ArgumentException("El GUID del parámetro es diferente al que viene en el campo agreement.guid.");

            var wrappedBson = new BsonDocument { { "UIConfigurationJson", bson } };

            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.guid", clientId),
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.workflowid", workflowID));

            await _collection.ReplaceOneAsync(filter, wrappedBson);
            return BsonSerializer.Deserialize<object>(wrappedBson);
        }

        public async Task DeleteIUConfiguration(string clientId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.guid", clientId);
            await _collection.DeleteOneAsync(filter);
            return;
        }

        public async Task<object?> GetGlobalConfiguration(string clientId, int workflowID)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.guid", clientId),
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.workflowid", workflowID));
            var projection = Builders<BsonDocument>.Projection.Include("UIConfigurationJson.global");
            var document = await _collection.Find(filter).Project(projection).FirstOrDefaultAsync();

            var global = document?["UIConfigurationJson"]?["global"];
            return global?.IsBsonDocument == true
                ? BsonSerializer.Deserialize<object>(global.AsBsonDocument)
                : null;
        }

        public async Task<object?> GetPageIfVisible(string clientId, string pageName, int workflowID)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.guid", clientId),
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.workflowid", workflowID));
            var projection = Builders<BsonDocument>.Projection.Include($"UIConfigurationJson.pages.{pageName}");
            var document = await _collection.Find(filter).Project(projection).FirstOrDefaultAsync();

            var page = document?["UIConfigurationJson"]?["pages"]?[pageName];
            if (page != null)
            {
                return BsonSerializer.Deserialize<object>(page.AsBsonDocument);
            }

            return null;
        }
        public async Task<List<string>> GetPagesWithDisplayTrue(string clientId, int workflowID)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.guid", clientId),
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.workflowid", workflowID));
            var projection = Builders<BsonDocument>.Projection.Include("UIConfigurationJson.pages");
            var document = await _collection.Find(filter).Project(projection).FirstOrDefaultAsync();

            var pages = document?["UIConfigurationJson"]?["pages"]?.AsBsonDocument;
            if (pages == null)
                return [];

            return [.. pages.Elements
                        .Where(e => e.Value["display"]?.AsBoolean == true)
                        .Select(e => e.Name)];
        }

        public async Task<object?> UpdateDynamicFieldsAsync(string clientId, int workflowID, Dictionary<string, object> updates)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.guid", clientId),
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.workflowid", workflowID)
            );

            var document = await _collection.Find(filter).FirstOrDefaultAsync();
            if (document == null)
                return null;

            var updateDefinitions = new List<UpdateDefinition<BsonDocument>>();

            foreach (var kv in updates)
            {
                var fieldPath = kv.Key;
                var newValue = kv.Value;

                // Validar si existe el campo en el documento
                if (!FieldExists(document, fieldPath))
                    continue; // Ignora campos inexistentes

                updateDefinitions.Add(
                    Builders<BsonDocument>.Update.Set(fieldPath, BsonValue.Create(newValue))
                );
            }

            if (updateDefinitions.Count == 0)
                return null; // Nada que actualizar

            var update = Builders<BsonDocument>.Update.Combine(updateDefinitions);
            await _collection.UpdateOneAsync(filter, update);

            // Devuelve documento actualizado
            var updatedDocument = await _collection.Find(filter).FirstOrDefaultAsync();
            return BsonSerializer.Deserialize<object>(updatedDocument);
        }

        private bool FieldExists(BsonDocument doc, string fieldPath)
        {
            var parts = fieldPath.Split('.');

            BsonValue current = doc;

            foreach (var part in parts)
            {
                if (current is BsonDocument bsonDoc && bsonDoc.Contains(part))
                {
                    current = bsonDoc[part];
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<object?> AddDynamicFieldsAsync(string clientId, int workflowID, Dictionary<string, object> fieldsToAdd)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.guid", clientId),
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.workflowid", workflowID)
            );

            var updates = new List<UpdateDefinition<BsonDocument>>();

            //En este proceso se valida el tipo de dato enviado y se convierte a BsonValue, para posteriormente agregarlo al documento

            foreach (var kvp in fieldsToAdd)
            {
                BsonValue bsonValue;

                if (kvp.Value == null)
                {
                    bsonValue = BsonNull.Value;
                }
                else if (kvp.Value is string s)
                {
                    bsonValue = new BsonString(s);
                }
                else if (kvp.Value is int n)
                {
                    bsonValue = new BsonInt32(n);
                }
                else if (kvp.Value is long l)
                {
                    bsonValue = new BsonInt64(l);
                }
                else if (kvp.Value is bool b)
                {
                    bsonValue = new BsonBoolean(b);
                }
                else if (kvp.Value is double d)
                {
                    bsonValue = new BsonDouble(d);
                }
                else
                {
                    bsonValue = BsonDocument.Parse(JsonSerializer.Serialize(kvp.Value));
                }

                updates.Add(
                    Builders<BsonDocument>.Update.Set(kvp.Key, bsonValue)
                );
            }

            if (!updates.Any())
                return null;

            var updateDefinition = Builders<BsonDocument>.Update.Combine(updates);

            var result = await _collection.UpdateOneAsync(filter, updateDefinition);

            return new
            {
                Modified = result.ModifiedCount,
                Upserted = result.UpsertedId != null
            };
        }

        public async Task<object?> AddIfNotExistsAsync(string clientId, int workflowID, Dictionary<string, object> fieldsToAdd)
        {
            var filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.guid", clientId),
                Builders<BsonDocument>.Filter.Eq("UIConfigurationJson.agreement.workflowid", workflowID)
            );

            var document = await _collection.Find(filter).FirstOrDefaultAsync();
            if (document == null)
                return null;

            var updates = new List<UpdateDefinition<BsonDocument>>();
            var skippedFields = new List<string>();

            foreach (var kvp in fieldsToAdd)
            {
                if (FieldExists(document, kvp.Key))
                {
                    skippedFields.Add(kvp.Key);
                    continue;
                }

                BsonValue bsonValue;

                if (kvp.Value == null)
                    bsonValue = BsonNull.Value;
                else if (kvp.Value is string s)
                    bsonValue = new BsonString(s);
                else if (kvp.Value is int i)
                    bsonValue = new BsonInt32(i);
                else if (kvp.Value is long l)
                    bsonValue = new BsonInt64(l);
                else if (kvp.Value is bool b)
                    bsonValue = new BsonBoolean(b);
                else if (kvp.Value is double d)
                    bsonValue = new BsonDouble(d);
                else
                    bsonValue = BsonDocument.Parse(JsonSerializer.Serialize(kvp.Value));

                updates.Add(
                    Builders<BsonDocument>.Update.Set(kvp.Key, bsonValue)
                );
            }

            if (!updates.Any())
            {
                return new
                {
                    Updated = false,
                    Reason = "All fields already exist",
                    Skipped = skippedFields
                };
            }

            var update = Builders<BsonDocument>.Update.Combine(updates);
            var result = await _collection.UpdateOneAsync(filter, update);

            return new
            {
                Updated = result.ModifiedCount > 0,
                AddedFields = updates.Count,
                Skipped = skippedFields
            };
        }

        public async Task<object> AddIfNotExistsToAllAsync(Dictionary<string, object> fieldsToAdd)
        {
            var documents = await _collection.Find(Builders<BsonDocument>.Filter.Empty).ToListAsync();

            long modified = 0;
            long skippedDocs = 0;

            foreach (var document in documents)
            {
                var updates = GetUpdatesForDocument(document, fieldsToAdd);

                if (!updates.Any())
                {
                    skippedDocs++;
                    continue;
                }

                var update = Builders<BsonDocument>.Update.Combine(updates);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", document["_id"]);
                var result = await _collection.UpdateOneAsync(filter, update);

                if (result.ModifiedCount > 0)
                    modified++;
            }

            return new
            {
                ModifiedDocuments = modified,
                SkippedDocuments = skippedDocs
            };
        }

        private List<UpdateDefinition<BsonDocument>> GetUpdatesForDocument(BsonDocument document, Dictionary<string, object> fieldsToAdd)
        {
            var updates = new List<UpdateDefinition<BsonDocument>>();

            foreach (var kvp in fieldsToAdd)
            {
                if (FieldExists(document, kvp.Key))
                    continue;

                var bsonValue = ConvertToBsonValue(kvp.Value);
                updates.Add(Builders<BsonDocument>.Update.Set(kvp.Key, bsonValue));
            }

            return updates;
        }

        private BsonValue ConvertToBsonValue(object? value)
        {
            return value switch
            {
                null => BsonNull.Value,
                string s => new BsonString(s),
                int i => new BsonInt32(i),
                long l => new BsonInt64(l),
                bool b => new BsonBoolean(b),
                double d => new BsonDouble(d),
                _ => BsonDocument.Parse(JsonSerializer.Serialize(value))
            };
        }
    }
}
