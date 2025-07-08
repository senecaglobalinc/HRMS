using HRMS.Common.Extensions;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Common.Redis
{
    /// <summary>
    /// This class contains all the redis get, set methods
    /// </summary>
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
            await m_Database.StringSetAsync(key, value);
        }

        /// <summary>
        /// Delete cache with the key
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        { 
            if (string.IsNullOrWhiteSpace(key) || key.Contains(":")) throw new ArgumentException("invalid key");

            m_Database.KeyDelete(key);
        }

        /// <summary>
        /// Gets the json value for a key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetStringValue(string key)
        {
            string cachedJson = m_Database.StringGet(key); 

            if (!string.IsNullOrEmpty(cachedJson))
            {                
                return cachedJson;
            }

            return default(string);
        }

        /// <summary>
        /// Set Hash
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objName"></param>
        public void HashSet(object obj, string objName)
        {
            m_Database.HashSet(objName, obj.ToHashEntries());
        }

        /// <summary>
        /// SetAdd
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetAdd(string key, string value)
        {
            m_Database.SetAdd(key, value);
        }

        /// <summary>
        /// HashGetAll
        /// </summary>
        /// <param name="key"></param>
        public HashEntry[] HashGetAll(string key)
        {
            if(m_Database.HashLength(key) > 0)
            {
                return m_Database.HashGetAll(key);
            }
            else
            {
                return null;
            }
           
        }

        /// <summary>
        /// SetMembers
        /// </summary>
        /// <param name="key"></param>
        public RedisValue[] SetMembers(string key)
        {
            return m_Database.SetMembers(key);
        }
    }
}
