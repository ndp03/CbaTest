
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Movies.Utility.Interfaces;

namespace Movies.Utility.Caching
{
    /// <summary>
    /// A simple implementation of the Cache using web Cache, this can be replace by other implementation
    /// without changing rest of the code
    /// </summary>
    public class HttpCacheProvider : ICacheProvider
    {
        public T GetCacheValue<T>(string cacheKey)
        {
            return HttpRuntime.Cache[cacheKey] != null
                ? (T)HttpRuntime.Cache[cacheKey]
                : default(T);
        }

        public T GetAndCacheValue<T>(string cacheKey, object cacheLock, Func<T> getValue, DateTime? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
        {
            var value = HttpRuntime.Cache[cacheKey];

            if (value == null)
            {
                lock (cacheLock)
                {
                    value = HttpRuntime.Cache[cacheKey];

                    if (value == null)
                    {
                        value = getValue();

                        if (value != null)
                        {
                            SetCacheValue(cacheKey, value, absoluteExpiration, slidingExpiration);
                        }
                    }
                }
            }

            return value != null ? (T)value : default(T);
        }

        public void RemoveCacheValue(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }

        public void RemoveCacheContains(string cacheKey)
        {
            var enumerator = HttpContext.Current.Cache.GetEnumerator();

            while (enumerator.MoveNext())
            {
                var key = enumerator.Key.ToString();

                if (key.Contains(cacheKey))
                {
                    HttpContext.Current.Cache.Remove(key);    
                }
            }
        }

        public void SetCacheValue<T>(string cacheKey, T value, DateTime? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
        {
            HttpRuntime.Cache.Insert(
                cacheKey,
                value,
                null,
                absoluteExpiration != null ? absoluteExpiration.Value : Cache.NoAbsoluteExpiration,
                slidingExpiration != null ? slidingExpiration.Value : Cache.NoSlidingExpiration,
                CacheItemPriority.Default,
                null
                );
        }
    }
}
