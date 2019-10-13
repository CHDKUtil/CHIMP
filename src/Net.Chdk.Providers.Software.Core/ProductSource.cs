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
            return obj is ProductSource source2 
                && Source == source2.Source;
        }

        public override int GetHashCode()
        {
            return Source.GetHashCode();
        }

        public static bool operator ==(ProductSource? source1, ProductSource? source2)
        {
            if (ReferenceEquals(source1, source2))
                return true;
            if (source1 is null || source2 is null)
                return false;
            return source1.Equals(source2);
        }

        public static bool operator !=(ProductSource? source1, ProductSource? source2)
        {
            return !(source1 == source2);
        }
    }
}
