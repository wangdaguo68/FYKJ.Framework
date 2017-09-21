namespace FYKJ.Config
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable]
    public class AdminMenuGroup
    {
        public List<AdminMenu> AdminMenuArray { get; set; }

        [XmlAttribute("icon")]
        public string Icon { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("info")]
        public string Info { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("permission")]
        public string Permission { get; set; }

        [XmlAttribute("url")]
        public string Url { get; set; }
    }
}

