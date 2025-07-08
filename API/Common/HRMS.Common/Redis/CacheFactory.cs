using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Common.Redis
{
    /// <summary>
    /// This class gets and sets the cache data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CacheFactory<T> : ICacheFactory<T>
    {
        private ICacheService m_CacheService;

        public CacheFactory(ICacheService cacheService)
        {
            m_CacheService = cacheService;
        }

        /// <summary>
        /// GetCacheObject
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> GetCacheObject(string key)
        {
            return await m_CacheService.GetCacheValueAsync<List<T>>(key);
        }

        
        /// <summary>
        /// SetCacheObject
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task SetCacheObject(string key, string value)
        {
            //Deletes the existing cache
            m_CacheService.Delete(key);
            await m_CacheService.SetCacheValueAsync(key, value);
        }
    }
}
