namespace FYKJ.Config
{
    using System;
    using Framework.Utility;

    public class ConfigContext
    {
        public ConfigContext() : this(new FileConfigService())
        {
        }

        public ConfigContext(IConfigService pageContentConfigService)
        {
            ConfigService = pageContentConfigService;
        }

        public virtual T Get<T>(string index = null) where T: ConfigFileBase, new()
        {
            var configFile = Activator.CreateInstance<T>();
            VilidateClusteredByIndex(configFile, index);
            return GetConfigFile<T>(index);
        }

        private T GetConfigFile<T>(string index = null) where T: ConfigFileBase, new()
        {
            var local = Activator.CreateInstance<T>();
            var configFileName = GetConfigFileName<T>(index);
            var config = ConfigService.GetConfig(configFileName);
            if (config == null)
            {
                ConfigService.SaveConfig(configFileName, string.Empty);
                return local;
            }
            if (string.IsNullOrEmpty(config))
            {
                return local;
            }
            try
            {
                return (T) SerializationHelper.XmlDeserialize(typeof(T), config);
            }
            catch
            {
                return Activator.CreateInstance<T>();
            }
        }

        public virtual string GetConfigFileName<T>(string index = null)
        {
            var name = typeof(T).Name;
            if (!string.IsNullOrEmpty(index))
            {
                name = string.Format("{0}_{1}", name, index);
            }
            return name;
        }

        public void Save<T>(T configFile, string index = null) where T: ConfigFileBase
        {
            VilidateClusteredByIndex(configFile, index);
            configFile.Save();
            var configFileName = GetConfigFileName<T>(index);
            ConfigService.SaveConfig(configFileName, SerializationHelper.XmlSerialize(configFile));
        }

        public virtual void VilidateClusteredByIndex<T>(T configFile, string index) where T: ConfigFileBase
        {
        }

        public IConfigService ConfigService { get; set; }
    }
}

