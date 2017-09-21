namespace FYKJ.Config
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable]
    public class UploadFolder
    {
        public UploadFolder()
        {
            Path = "Default";
            DirType = DirType.Day;
        }

        [XmlAttribute("DirType")]
        public DirType DirType { get; set; }

        [XmlAttribute("Path")]
        public string Path { get; set; }

        public List<ThumbnailSize> ThumbnailSizes { get; set; }
    }
}

