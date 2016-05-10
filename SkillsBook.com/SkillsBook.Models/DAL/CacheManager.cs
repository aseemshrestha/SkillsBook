using System.Security.Cryptography.X509Certificates;
using SkillsBook.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace SkillsBook.Models.DAL
{
    public class CacheManager
    {
        protected static readonly MemoryCache Cache = MemoryCache.Default;

      
        public static bool Exists(string cacheKey)
        {
            var exists = Cache.Get(cacheKey) != null;
            return exists;
        }

        public static void AddToCache<T>(string cacheKey, object result=null, List<T> resultList=null, int hours=0)
        {
            
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddHours(hours)
            };

            if (result != null)
            {
                Cache.Add(cacheKey, result, cacheItemPolicy);
            }
            else if(resultList!=null)
            {
                Cache.Add(cacheKey, resultList, cacheItemPolicy);
            }
        }

        public static void ClearAllCache()
        {
            var cacheKeys = Cache.Select(kvp => kvp.Key).ToList();
            foreach (var cacheKey in cacheKeys)
            {
               Cache.Remove(cacheKey);
            }
        }

        public static void ClearSpecificCacheObject(string cacheKey)
        {
            
           Cache.Remove(cacheKey);
        }

        public static object Get(string key)
        {
           var output =  Cache.Get(key);
           return output;
        }

        /* private methods*/
        protected static void Refresh<T>(string cacheKey, object result = null, Task<List<T>> resultsList = null)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddDays(1)
            };
            if (result != null)
            {
                Cache.Add(cacheKey, result, cacheItemPolicy);
            }
            else if (resultsList != null)
            {
                Cache.Add(cacheKey, resultsList, cacheItemPolicy);
            }
        }
        protected static void RefreshSync<T>(string cacheKey, object result = null, List<T> resultsList = null)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddDays(1)
            };
            if (result != null)
            {
                Cache.Add(cacheKey, result, cacheItemPolicy);
            }
            else if (resultsList != null)
            {
                Cache.Add(cacheKey, resultsList, cacheItemPolicy);
            }
        }
        
       
    }
}
