using System.Web.Caching;

namespace FYKJ.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Web;
    using Framework.Utility;

    public class LocalCacheProvider : ICacheProvider
    {
        public virtual void Clear(string keyRegex)
        {
            var list = new List<string>();
            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var input = enumerator.Key.ToString();
                if (Regex.IsMatch(input, keyRegex, RegexOptions.IgnoreCase))
                {
                    list.Add(input);
                }
            }
            foreach (var t in list)
            {
                HttpRuntime.Cache.Remove(t);
            }
        }

        public virtual object Get(string key)
        {
            return Caching.Get(key);
        }

        public virtual void Remove(string key)
        {
            Caching.Remove(key);
        }

        public virtual void Set(string key, object value, int minutes, bool isAbsoluteExpiration, Action<string, object, string> onRemove)
        {
            Caching.Set(key, value, minutes, isAbsoluteExpiration, delegate (string k, object v, CacheItemRemovedReason reason) {
                onRemove?.Invoke(k, v, reason.ToString());
            });
        }
    }
}

