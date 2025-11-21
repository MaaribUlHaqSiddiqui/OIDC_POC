using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserActivity
{
    public class UserActivity: IUserActivity
    {
        private readonly ConnectionMultiplexer _multiplexer;
        readonly IDatabase _database;

        public UserActivity(ConnectionMultiplexer multiplexer)
        {
            _multiplexer = multiplexer;
            _database = multiplexer.GetDatabase();

        }

        public async Task LogActivityAsync(string userId, string activity)
        {
            var key = $"activity_{userId}";
            //bool result=await _database.StringSetAsync (key, activity,expiry:TimeSpan.FromSeconds(100));
            await _database.ListRightPushAsync(key, activity);

        }

        public async Task<IEnumerable<string>> GetActivityAsync(string userId)
        {
            var key = $"activity_{userId}";
            //return await _database.StringGetAsync(key);

            var activities = await _database.ListRangeAsync(key);
            return activities.Select(a => a.ToString());
        }
    }
}
