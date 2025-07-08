using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Common.Redis
{
    /// <summary>
    /// Interface for cacheservice with abstract methods
    /// </summary>
    public interface ICacheService
    {
        string GetStringValue(string key);
        T GetCacheValue<T>(string key);
        Task<T> GetCacheValueAsync<T>(string key);
        Task SetCacheValueAsync(string key, string value);
        void Delete(string key);
        void HashSet(object key, string objName);
        void SetAdd(string key, string value);
        HashEntry[] HashGetAll(string key);
        RedisValue[] SetMembers(string key);
    }
}
