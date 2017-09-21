namespace FYKJ.Config
{
    using System;

    [Serializable]
    public class SystemConfig : ConfigFileBase
    {
        public int UserLoginTimeoutMinutes { get; set; }
    }
}

