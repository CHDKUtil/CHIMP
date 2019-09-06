using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software
{
    public sealed class ProductSource
    {
        public ProductSource(string productName, string sourceName, SoftwareSourceInfo source)
        {
            ProductName = productName;
            SourceName = sourceName;
            Source = source;
        }

        public string ProductName { get; }
        public string SourceName { get; }
        public SoftwareSourceInfo Source { get; }

        public override bool Equals(object obj)
        {
            var source2 = obj as ProductSource;
            if (source2 == null)
                return false;
            return Source.Equals(source2.Source);
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode();
        }
    }
}
