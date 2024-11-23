using System.Text.Json;
using StackExchange.Redis;
using MongoDB.Driver;
using web_app_domain;

namespace web_app_repository
{
    public class CachedConsumoRepository : IConsumoRepository
    {
        private readonly IMongoCollection<Consumo> _consumoCollection;
        private readonly IDatabase _cache;

        public CachedConsumoRepository(IMongoClient mongoClient, IOptions<MongoDbSettings> settings, IConnectionMultiplexer redis)
        {
            var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _consumoCollection = database.GetCollection<Consumo>("consumptions");
            _cache = redis.GetDatabase();
        }

        public async Task<IEnumerable<Consumo>> ListConsumptions()
        {
            const string cacheKey = "consumptions";

            
            var cachedData = await _cache.StringGetAsync(cacheKey);
            if (!cachedData.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<List<Consumo>>(cachedData);
            }

           
            var consumptions = await _consumoCollection.Find(_ => true).ToListAsync();

            
            await _cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(consumptions), TimeSpan.FromMinutes(5));

            return consumptions;
        }

        public async Task SaveConsumption(Consumo consumption)
        {
            await _consumoCollection.InsertOneAsync(consumption);

            await _cache.KeyDeleteAsync("consumptions");
        }

        public async Task UpdateConsumption(Consumo consumption)
        {
            var filter = Builders<Consumo>.Filter.Eq(c => c.Id, consumption.Id);
            await _consumoCollection.ReplaceOneAsync(filter, consumption);


            await _cache.KeyDeleteAsync("consumptions");
        }

        public async Task RemoveConsumption(string id)
        {
            var filter = Builders<Consumo>.Filter.Eq(c => c.Id, int.Parse(id));
            await _consumoCollection.DeleteOneAsync(filter);

            
            await _cache.KeyDeleteAsync("consumptions");
        }
    }
}
