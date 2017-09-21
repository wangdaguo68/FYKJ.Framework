using System;
using System.Web;
using System.Web.Caching;

namespace FYKJ.Framework.Utility
{
    public class Caching
    {
        public static object Get(string name)
        {
            return HttpRuntime.Cache.Get(name);
        }

        public static void Remove(string name)
        {
            if (HttpRuntime.Cache[name] != null)
            {
                HttpRuntime.Cache.Remove(name);
            }
        }

        public static void Set(string name, object value)
        {
            Set(name, value, null);
        }

        public static void Set(string name, object value, int minutes)
        {
            HttpRuntime.Cache.Insert(name, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes));
        }

        public static void Set(string name, object value, CacheDependency cacheDependency)
        {
            HttpRuntime.Cache.Insert(name, value, cacheDependency, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20.0));
        }

        public static void Set(string name, object value, int minutes, bool isAbsoluteExpiration, CacheItemRemovedCallback onRemoveCallback)
        {
            if (isAbsoluteExpiration)
            {
                HttpRuntime.Cache.Insert(name, value, null, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration, CacheItemPriority.Normal, onRemoveCallback);
            }
            else
            {
                HttpRuntime.Cache.Insert(name, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes), CacheItemPriority.Normal, onRemoveCallback);
            }
        }
    }
}

