namespace Net.Chdk.Meta.Model
{
    public abstract class SourceData<TSource>
        where TSource : SourceData<TSource>
    {
        public string? Revision { get; set; }
    }

    public sealed class PlatformSourceData : SourceData<PlatformSourceData>
    {
        public string? Platform { get; set; }
    }
}
