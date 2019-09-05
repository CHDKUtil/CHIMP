using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Model.Camera
{
    public abstract class CameraModelData : ICameraModelData
    {
        public string[] Names { get; set; }
        public string Platform { get; set; }

        IDictionary<string, IRevisionData> ICameraModelData.Revisions => GetRevisionsInternal();

        internal abstract IDictionary<string, IRevisionData> GetRevisionsInternal();
    }

    public abstract class CameraModelData<TRevision> : CameraModelData
        where TRevision : IRevisionData
    {
        internal sealed override IDictionary<string, IRevisionData> GetRevisionsInternal()
        {
            return GetRevisions()
                .ToDictionary(kvp => kvp.Key, kvp => (IRevisionData)kvp.Value);
        }

        protected abstract IDictionary<string, TRevision> GetRevisions();
    }
}
