using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Model.Camera
{
    public abstract class CameraModelData : ICameraModelData
    {
        public string[] Names { get; set; }
        public string Platform { get; set; }

        IDictionary<string, IRevisionData> ICameraModelData.Revisions
        {
            get => GetRevisionsInternal();
            set => SetRevisions(value);
        }

        internal abstract IDictionary<string, IRevisionData> GetRevisionsInternal();
        internal abstract void SetRevisions(IDictionary<string, IRevisionData> value);
    }

    public abstract class CameraModelData<TRevision> : CameraModelData
        where TRevision : IRevisionData
    {
        internal sealed override IDictionary<string, IRevisionData> GetRevisionsInternal()
        {
            return GetRevisions()
                .ToDictionary(kvp => kvp.Key, kvp => (IRevisionData)kvp.Value);
        }

        internal sealed override void SetRevisions(IDictionary<string, IRevisionData> value)
        {
            var revisions = value
                .ToDictionary(kvp => kvp.Key, kvp => (TRevision)kvp.Value);
            SetRevisions(revisions);
        }

        protected abstract IDictionary<string, TRevision> GetRevisions();
        protected abstract void SetRevisions(IDictionary<string, TRevision> value);
    }
}
