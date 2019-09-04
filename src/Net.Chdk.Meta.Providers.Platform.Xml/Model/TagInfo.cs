using System.Xml.Serialization;

namespace Net.Chdk.Meta.Providers.Platform.Xml.Model
{
    [XmlType("taginfo")]
    public sealed class TagInfo
    {
        [XmlElement("table")]
        public Table[] Tables { get; set; }
    }
}
