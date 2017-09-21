namespace FYKJ.Config
{
    using System;

    [Serializable]
    public class AdminMenuConfig : ConfigFileBase
    {
        public AdminMenuGroup[] AdminMenuGroups { get; set; }
    }
}

