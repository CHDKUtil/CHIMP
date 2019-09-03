namespace Net.Chdk.Meta.Model
{
    public abstract class RevisionData<TRevision, TSource>
        where TRevision : RevisionData<TRevision, TSource>
        where TSource : SourceData<TSource>
    {
        public TSource Source { get; set; }
    }
}
