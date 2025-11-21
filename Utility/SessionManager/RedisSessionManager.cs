using System.Text.Json;
using StackExchange.Redis;

namespace SessionManager
{
    public class RedisSessionManager
    {
        private readonly IDatabase _redisDatabase;
        private readonly IConnectionMultiplexer _redis;

        public RedisSessionManager(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _redisDatabase = redis.GetDatabase();
        }

        public async Task StoreTokenAsync(string userId, string token, TimeSpan expiration)
        {
            await _redisDatabase.StringSetAsync(userId, token, expiration);
        }

        public async Task<string?> GetTokenAsync(string userId)
        {
            return await _redisDatabase.StringGetAsync(userId);
        }

        public async Task StoreAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _redisDatabase.StringSetAsync(key, json, expiration);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var json = await _redisDatabase.StringGetAsync(key);
            return json.HasValue ? JsonSerializer.Deserialize<T>(json!) : default;
        }

        public async Task<bool> RemoveTokenAsync(string userId)
        {
            return await _redisDatabase.KeyDeleteAsync(userId);
        }
    }
}
