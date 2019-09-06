using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Model.Camera
{
    public abstract class CameraModelData<TModel, TRevision> : ICameraModelData<TModel, TRevision>, ICameraModelData
        where TModel : CameraModelData<TModel, TRevision>
        where TRevision : IRevisionData
    {
        public string[] Names { get; set; }
        public string Platform { get; set; }

        IDictionary<string, IRevisionData> ICameraModelData.Revisions
        {
            get
            {
                return GetRevisions()
                    .ToDictionary(kvp => kvp.Key, kvp => (IRevisionData)kvp.Value);
            }

            set
            {
                var revisions = value
                    .ToDictionary(kvp => kvp.Key, kvp => (TRevision)kvp.Value);
                SetRevisions(revisions);
            }
        }

        IDictionary<string, TRevision> ICameraModelData<TModel, TRevision>.Revisions
        {
            get => GetRevisions();
            set { SetRevisions(value); }
        }

        protected abstract IDictionary<string, TRevision> GetRevisions();
        protected abstract void SetRevisions(IDictionary<string, TRevision> value);
    }
}
