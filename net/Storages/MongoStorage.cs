using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace net.Storages
{
    public class MongoStorage
    {
        private readonly string _mongoURI = "mongodb://127.0.0.1:27017";
        private readonly string _databaseName = "filtex";
        private readonly string _collectionName = "projects";

        public async Task Init()
        {
            var client = new MongoClient(_mongoURI);
            var database = client.GetDatabase(_databaseName);
            
            await database.DropCollectionAsync(_collectionName);
            
            var collection = database.GetCollection<BsonDocument>(_collectionName);

            var docs = new List<BsonDocument>();
            foreach (var v in Data.List)
            {
                docs.Add(new BsonDocument()
                {
                    { "name", v.name },
                    { "version", v.version },
                    { "tags", BsonValue.Create(v.tags) },
                    { "status", v.status },
                    { "createdAt", v.createdAt },
                });   
            }

            await collection.InsertManyAsync(docs);
        }
        
        public async Task<List<Dictionary<string, object>>> Query(BsonDocument query)
        {
            var client = new MongoClient(_mongoURI);
            var database = client.GetDatabase(_databaseName);
            
            var collection = database.GetCollection<BsonDocument>(_collectionName);

            var cursor = await collection.FindAsync(query);
            var list = await cursor.ToListAsync();

            var results = new List<Dictionary<string, object>>();
            
            foreach (var v in list)
            {
                results.Add(new Dictionary<string, object>
                {
                    { "id", v["_id"].AsObjectId.ToString() },
                    { "name", v["name"].AsString },
                    { "tags", v["tags"].AsBsonArray.Select(x => x.AsString) },
                    { "version", v["version"].AsInt32 },
                    { "status", v["status"].AsBoolean },
                    { "createdAt", v["createdAt"].ToUniversalTime() }
                });
            }

            return results;
        }
    }
}