using RedisAPI.Models;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text.Json;

namespace RedisAPI.Data
{
    public class RedisPlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisPlatformRepo(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform is null)
            {
                throw new ArgumentException(nameof(platform));
            }
            
            var serializePlatform = JsonSerializer.Serialize(platform);

            var db = _connectionMultiplexer.GetDatabase();

            //db.StringSet(platform.Id, serializePlatform);
            //db.SetAdd("PlatformSet", serializePlatform);
            //

            db.HashSet("hashplatform", new HashEntry[]{ new HashEntry(platform.Id, serializePlatform)});
        }

        public IEnumerable<Platform?>? GetAllPlatforms()
        {
            var db = _connectionMultiplexer.GetDatabase();

            // var completeSet = db.SetMembers("PlatformSet");

            var completeHash = db.HashGetAll("hashplatform");

            if (completeHash.Length > 0)
            {
                var platforms = Array.ConvertAll(completeHash, x => JsonSerializer.Deserialize<Platform>(x.Value)).ToList();

                return platforms;
            }

            return null;
        }

        public Platform? GetPlatformById(string id)
        {
            var db = _connectionMultiplexer.GetDatabase();
            //var serializePlatform = db.StringGet(id);

            var serializePlatform = db.HashGet("hashplatform", id);

            if (!string.IsNullOrEmpty(serializePlatform))
            {
                var platform = JsonSerializer.Deserialize<Platform>(serializePlatform);
                return platform;
            }

            return null;
        }
    }
}
