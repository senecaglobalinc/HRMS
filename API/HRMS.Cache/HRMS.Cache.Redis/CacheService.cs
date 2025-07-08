using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Cache.Redis
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer m_ConnectionMultiplexer;
        private readonly IDatabase m_Database;

        public CacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            m_ConnectionMultiplexer = connectionMultiplexer;
            m_Database = m_ConnectionMultiplexer.GetDatabase();
        }

        /// <summary>
        /// Get the Cache value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetCacheValueAsync<T>(string key)
        {
            //var database = m_ConnectionMultiplexer.GetDatabase();
            string cachedJson = await m_Database.StringGetAsync(key);

            // If there was a cached item then deserialise this
            if (!string.IsNullOrEmpty(cachedJson))
            {
                T cachedObject = JsonConvert.DeserializeObject<T>(cachedJson);
                return cachedObject;
            }

            return default(T);

        }

        /// <summary>
        /// Get the Cache value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetCacheValue<T>(string key)
        {
            //var database = m_ConnectionMultiplexer.GetDatabase();
            var cachedJson = m_Database.StringGet(key);

            // If there was a cached item then deserialise this
            if (!string.IsNullOrEmpty(cachedJson))
            {
                T cachedObject = JsonConvert.DeserializeObject<T>(cachedJson);
                return cachedObject;
            }

            return default(T);

        }

        /// <summary>
        /// Set Cache value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetCacheValueAsync(string key, string value)
        {
            //var database = m_ConnectionMultiplexer.GetDatabase();
            await m_Database.StringSetAsync(key, value);
        }

        /// <summary>
        /// Delete cache with the key
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            //var database = m_ConnectionMultiplexer.GetDatabase();
            if (string.IsNullOrWhiteSpace(key) || key.Contains(":")) throw new ArgumentException("invalid key");

            m_Database.KeyDelete(key);
        }

        public void HashSet(object obj, string objName)
        {
            m_Database.HashSet(objName, obj.ToHashEntries());
        }
        
        public void SetAdd(string key, string value)
        {
            m_Database.SetAdd(key, value);
        }


    }
}
