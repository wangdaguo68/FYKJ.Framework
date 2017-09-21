namespace FYKJ.Config
{
    using System;

    [Serializable]
    public class DaoConfig : ConfigFileBase
    {
        public string EnterprisePlat { get; set; }

        public string Log { get; set; }
    }
}

