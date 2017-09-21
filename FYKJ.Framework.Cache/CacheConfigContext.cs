namespace FYKJ.Cache
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Config;

    internal class CacheConfigContext
    {
        private static Dictionary<string, ICacheProvider> cacheProviders;
        private static string moduleName;
        private static readonly object olock = new object();
        private static Dictionary<string, WrapCacheConfigItem> wrapCacheConfigItemDic;
        private static List<WrapCacheConfigItem> wrapCacheConfigItems;

        internal static WrapCacheConfigItem GetCurrentWrapCacheConfigItem(string key)
        {
            if (wrapCacheConfigItemDic == null)
            {
                wrapCacheConfigItemDic = new Dictionary<string, WrapCacheConfigItem>();
            }
            if (wrapCacheConfigItemDic.ContainsKey(key))
            {
                return wrapCacheConfigItemDic[key];
            }
            WrapCacheConfigItem item = (from i in WrapCacheConfigItems
                                        where Regex.IsMatch(ModuleName, i.CacheConfigItem.ModuleRegex, RegexOptions.IgnoreCase) && 
                                        Regex.IsMatch(key, i.CacheConfigItem.KeyRegex, RegexOptions.IgnoreCase)
                                        orderby i.CacheConfigItem.Priority descending
                                        select i).FirstOrDefault();
            if (item == null)
            {
                throw new Exception(string.Format("获取缓存 '{0}' 异常", key));
            }
            lock (olock)
            {
                if (!wrapCacheConfigItemDic.ContainsKey(key))
                {
                    wrapCacheConfigItemDic.Add(key, item);
                }
            }
            return item;
        }   

        internal static CacheConfig CacheConfig => CachedConfigContext.Current.CacheConfig;

        internal static Dictionary<string, ICacheProvider> CacheProviders
        {
            get
            {
                if (cacheProviders == null)
                {
                    lock (olock)
                    {
                        if (cacheProviders == null)
                        {
                            cacheProviders = new Dictionary<string, ICacheProvider>();
                            foreach (CacheProviderItem item in CacheConfig.CacheProviderItems)
                            {
                                var type = item!=null ? Type.GetType(item.Type) : null;
                                if (type!=null)
                                {
                                    cacheProviders.Add(item.Name, (ICacheProvider) Activator.CreateInstance(type));
                                }
                            }
                        }
                    }
                }
                return cacheProviders;
            }
        }

        public static string ModuleName
        {
            get
            {
                if (moduleName == null)
                {
                    lock (olock)
                    {
                        if (moduleName == null)
                        {
                            Assembly entryAssembly = Assembly.GetEntryAssembly();
                            moduleName = entryAssembly?.FullName ?? new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Name;
                        }
                    }
                }
                return moduleName;
            }
        }

        internal static List<WrapCacheConfigItem> WrapCacheConfigItems
        {
            get
            {
                if (wrapCacheConfigItems == null)
                {
                    lock (olock)
                    {
                        if (wrapCacheConfigItems != null) return wrapCacheConfigItems;
                        wrapCacheConfigItems = new List<WrapCacheConfigItem>();
                        var cacheConfigItems = CacheConfig.CacheConfigItems;
                        if (cacheConfigItems==null)
                        {
                            return new List<WrapCacheConfigItem>();
                        }
                        foreach (var t in cacheConfigItems)
                        {
                            var i = t;
                            var item = new WrapCacheConfigItem
                            {
                                CacheConfigItem = i
                            };
                            Func<CacheProviderItem, bool> predicate = c => c.Name == i.ProviderName;
                            item.CacheProviderItem = CacheConfig.CacheProviderItems.SingleOrDefault(predicate);
                            item.CacheProvider = CacheProviders[i.ProviderName];
                            wrapCacheConfigItems.Add(item);
                        }
                    }
                }
                return wrapCacheConfigItems;
            }
        }
    }
}

