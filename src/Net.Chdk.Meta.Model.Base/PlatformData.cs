using System.Collections.Generic;

namespace Net.Chdk.Meta.Model
{
    public abstract class PlatformData<TPlatform, TRevision, TSource>
        where TPlatform : PlatformData<TPlatform, TRevision, TSource>
        where TRevision : RevisionData<TRevision, TSource>
        where TSource : SourceData<TSource>
    {
        public IDictionary<string, TRevision> Revisions { get; set; }
    }
}
