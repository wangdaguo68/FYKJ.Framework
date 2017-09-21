namespace FYKJ.Cache
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;

    public class CacheHelper
    {
        public static void Clear(string keyRegex = ".*", string moduleRegex = ".*")
        {
            if (!Regex.IsMatch(CacheConfigContext.ModuleName, moduleRegex, RegexOptions.IgnoreCase)) return;
            foreach (var provider in CacheConfigContext.CacheProviders.Values)
            {
                provider.Clear(keyRegex);
            }
        }

        public static object Get(string key)
        {
            var cache = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
            return cache?.CacheProvider.Get(key);
        }

        public static F Get<F>(string key, Func<F> getRealData)
        {
            Func<F> func = delegate
            {
                var obj2 = Get(key);
                if (obj2 != null) return (F)obj2;
                var local = getRealData();
                if (local != null)
                {
                    Set(key, local);
                }
                return local;
            };
            return GetItem(key, func);
        }

        public static F Get<F>(string key, int id, Func<int, F> getRealData)
        {
            return Get<F, int>(key, id, getRealData);
        }

        public static F Get<F>(string key, string branchKey, Func<F> getRealData)
        {
            return Get<F, string>(key, branchKey, id => getRealData());
        }

        public static F Get<F>(string key, string id, Func<string, F> getRealData)
        {
            return Get<F, string>(key, id, getRealData);
        }

        public static F Get<F, T>(string key, T id, Func<T, F> getRealData)
        {
            key = string.Format("{0}_{1}", key, id);
            Func<F> func = delegate
            {
                var obj2 = Get(key);
                if (obj2 != null) return (F)obj2;
                var local = getRealData(id);
                if (local != null)
                {
                    Set(key, local);
                }
                return local;
            };
            return GetItem(key, func);
        }

        public static F GetItem<F>() where F : new()
        {
            return GetItem(typeof(F).ToString(), () => new F());
        }

        public static F GetItem<F>(Func<F> getRealData)
        {
            return GetItem(typeof(F).ToString(), getRealData);
        }

        public static F GetItem<F>(string name, Func<F> getRealData)
        {
            if (HttpContext.Current == null)
            {
                return getRealData();
            }
            var items = HttpContext.Current.Items;
            if (items.Contains(name))
            {
                return (F)items[name];
            }
            F local = getRealData();
            if (local != null)
            {
                items[name] = local;
            }
            return local;
        }

        public static void Remove(string key)
        {
            CacheConfigContext.GetCurrentWrapCacheConfigItem(key).CacheProvider.Remove(key);
        }

        public static void Set(string key, object value)
        {
            var currentWrapCacheConfigItem = CacheConfigContext.GetCurrentWrapCacheConfigItem(key);
            currentWrapCacheConfigItem?.CacheProvider.Set(key, value,
                currentWrapCacheConfigItem.CacheConfigItem.Minitus,
                currentWrapCacheConfigItem.CacheConfigItem.IsAbsoluteExpiration, null);
        }
    }
}

