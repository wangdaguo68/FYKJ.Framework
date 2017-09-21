namespace FYKJ.Config
{
    using System.Web.Caching;
    using Framework.Utility;

    public class CachedConfigContext : ConfigContext
    {
        public static CachedConfigContext Current = new CachedConfigContext();

        public override T Get<T>(string index = null)
        {
            var configFileName = GetConfigFileName<T>(index);
            var name = "ConfigFile_" + configFileName;
            var obj2 = Caching.Get(name);
            if (obj2 != null)
            {
                return (T) obj2;
            }
            var local = base.Get<T>(index);
            Caching.Set(name, local, new CacheDependency(ConfigService.GetFilePath(configFileName)));
            return local;
        }

        public AdminMenuConfig AdminMenuConfig => Get<AdminMenuConfig>();

        public CacheConfig CacheConfig => Get<CacheConfig>();

        public DaoConfig DaoConfig => Get<DaoConfig>();

        public SettingConfig SettingConfig => Get<SettingConfig>();

        public SystemConfig SystemConfig => Get<SystemConfig>();

        public UploadConfig UploadConfig => Get<UploadConfig>();
    }
}

