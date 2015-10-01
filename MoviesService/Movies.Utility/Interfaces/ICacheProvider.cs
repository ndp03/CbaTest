using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Utility.Interfaces
{
    public interface ICacheProvider
    {
        T GetAndCacheValue<T>(string cacheKey, object cacheLock, Func<T> getValue, DateTime? absoluteExpiration = null, TimeSpan? slidingExpiration = null);

        T GetCacheValue<T>(string cacheKey);

        void RemoveCacheValue(string cacheKey);

        void RemoveCacheContains(string cacheKey);

        void SetCacheValue<T>(string cacheKey, T value, DateTime? absoluteExpiration = null, TimeSpan? slidingExpiration = null);
    }
}
