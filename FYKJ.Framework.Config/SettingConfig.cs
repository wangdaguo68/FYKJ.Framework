namespace FYKJ.Config
{
    using System;

    [Serializable]
    public class SettingConfig : ConfigFileBase
    {
        public string WebSiteDescription { get; set; }

        public string WebSiteKeywords { get; set; }

        public string WebSiteTitle { get; set; }
    }
}

