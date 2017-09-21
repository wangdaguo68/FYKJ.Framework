namespace FYKJ.Config
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable]
    public class UploadConfig : ConfigFileBase
    {
        public List<UploadFolder> UploadFolders { get; set; }

        [XmlAttribute("UploadPath")]
        public string UploadPath { get; set; }
    }
}

