using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Common.Redis
{
    /// <summary>
    /// Interface for Cache factory
    /// </summary>
    /// <typeparam name="T"></typeparam>
   public interface ICacheFactory<T>
    {
        Task<List<T>> GetCacheObject(string key);

        Task SetCacheObject(string key, string value);
    }
}
