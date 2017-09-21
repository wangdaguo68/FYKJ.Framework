namespace FYKJ.Config
{
    using System;

    [Serializable]
    public class CacheConfig : ConfigFileBase
    {
        public CacheConfigItem[] CacheConfigItems { get; set; }

        public CacheProviderItem[] CacheProviderItems { get; set; }
    }
}

