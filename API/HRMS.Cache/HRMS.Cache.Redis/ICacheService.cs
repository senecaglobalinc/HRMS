using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Cache.Redis
{
    public interface ICacheService
    {
        T GetCacheValue<T>(string key);
        Task<T> GetCacheValueAsync<T>(string key);
        Task SetCacheValueAsync(string key, string value);
        void Delete(string key);

        void HashSet(object obj, string objName);
        void SetAdd(string obj, string objName);

    }
}
