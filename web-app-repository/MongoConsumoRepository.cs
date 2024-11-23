using MongoDB.Driver;
using MongoDB.Bson;
using web_app_domain;
using Microsoft.Extensions.Options;

namespace web_app_repository
{
    public class MongoConsumoRepository : IConsumoRepository
    {
        private readonly IMongoCollection<Consumo> _consumoCollection;

        public MongoConsumoRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _consumoCollection = database.GetCollection<Consumo>("consumptions");
        }

        public async Task<IEnumerable<Consumo>> ListConsumptions()
        {
            return await _consumoCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task SaveConsumption(Consumo consumption)
        {
            await _consumoCollection.InsertOneAsync(consumption);
        }

        public async Task UpdateConsumption(Consumo consumption)
        {
            var filter = Builders<Consumo>.Filter.Eq(c => c.Id, consumption.Id);
            await _consumoCollection.ReplaceOneAsync(filter, consumption);
        }

        public async Task RemoveConsumption(string id)
        {
            var filter = Builders<Consumo>.Filter.Eq(c => c.Id, int.Parse(id));
            await _consumoCollection.DeleteOneAsync(filter);
        }
    }
}
