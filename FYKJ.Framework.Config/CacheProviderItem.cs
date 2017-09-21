namespace FYKJ.Config
{
    using System.Xml.Serialization;

    public class CacheProviderItem : ConfigNodeBase
    {
        [XmlAttribute(AttributeName="desc")]
        public string Desc { get; set; }

        [XmlAttribute(AttributeName="name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName="type")]
        public string Type { get; set; }
    }
}

