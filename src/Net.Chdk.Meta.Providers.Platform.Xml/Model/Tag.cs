using System.Xml.Serialization;

namespace Net.Chdk.Meta.Providers.Platform.Xml.Model
{
    public sealed class Tag
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlArray("values")]
        public Key[] Values { get; set; }
    }
}
