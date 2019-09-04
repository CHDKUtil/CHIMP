using System.Xml.Serialization;

namespace Net.Chdk.Meta.Providers.Platform.Xml.Model
{
    [XmlType("key")]
    public sealed class Key
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("val")]
        public string Value { get; set; }
    }
}
