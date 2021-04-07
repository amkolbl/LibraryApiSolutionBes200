using LibraryApi.Controllers;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static LibraryApi.Controllers.CachingController;

namespace LibraryApi.Services
{
    public class RedisOnCallDeveloperLookup : ILookupOnCallDevelopers
    {
        private readonly IDistributedCache _cache;

        public RedisOnCallDeveloperLookup(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<CachingController.OnCallDeveloperResponse> GetOnCallDeveloperAsync()
        {
            //check to see if it is in the cache
            var cachedResponse = await _cache.GetAsync("oncall");
            if(cachedResponse != null)
            {
                //if it is there, we return that thing
                string storedResponse = Encoding.UTF8.GetString(cachedResponse);
                // "{ "name": "Ryan",..... }"
                OnCallDeveloperResponse response = JsonSerializer.Deserialize <OnCallDeveloperResponse>(storedResponse);
                return response;
            }
            //if it isn't, we:
            else
            {
                // - do the work to recreate it
                var dev = new OnCallDeveloperResponse
                {
                    Name = "Ryan",
                    Email = "ryan@compuserve.com",
                    Until = DateTime.Now.AddHours(12)
                };
                // - put it in the cache
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddSeconds(15));
                string serializedDev = JsonSerializer.Serialize(dev);
                byte[] encodedDev = Encoding.UTF8.GetBytes(serializedDev);
                await _cache.SetAsync("oncall", encodedDev, options);
                // - return that thing
                return dev;
            }

        }
    }
}
