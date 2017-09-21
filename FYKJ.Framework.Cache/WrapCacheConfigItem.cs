namespace FYKJ.Cache
{
    public class WrapCacheConfigItem
    {
        public Config.CacheConfigItem CacheConfigItem { get; set; }

        public ICacheProvider CacheProvider { get; set; }

        public Config.CacheProviderItem CacheProviderItem { get; set; }
    }
}

