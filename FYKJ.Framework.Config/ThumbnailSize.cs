namespace FYKJ.Config
{
    using System;
    using System.Xml.Serialization;
    using Framework.Utility;

    [Serializable]
    public class ThumbnailSize
    {
        public ThumbnailSize()
        {
            Quality = 0x58;
            Mode = "Cut";
            Timming = Timming.Lazy;
            WaterMarkerPosition = ImagePosition.Default;
        }

        [XmlAttribute("AddWaterMarker")]
        public bool AddWaterMarker { get; set; }

        [XmlAttribute("Height")]
        public int Height { get; set; }

        [XmlAttribute("IsReplace")]
        public bool IsReplace { get; set; }

        [XmlAttribute("Mode")]
        public string Mode { get; set; }

        [XmlAttribute("Quality")]
        public int Quality { get; set; }

        [XmlAttribute("Timming")]
        public Timming Timming { get; set; }

        [XmlAttribute("WaterMarkerPath")]
        public string WaterMarkerPath { get; set; }

        [XmlAttribute("WaterMarkerPosition")]
        public ImagePosition WaterMarkerPosition { get; set; }

        [XmlAttribute("Width")]
        public int Width { get; set; }
    }
}

