using System.Xml.Serialization;

namespace Net.Chdk.Meta.Providers.Platform.Xml.Model
{
    public sealed class Table
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("tag")]
        public Tag[] Tags { get; set; }
    }
}
