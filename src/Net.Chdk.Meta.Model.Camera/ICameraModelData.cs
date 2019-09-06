using System.Collections.Generic;

namespace Net.Chdk.Meta.Model.Camera
{
    public interface ICameraModelData
    {
        string[] Names { get; }
        string Platform { get; }
        IDictionary<string, IRevisionData> Revisions { get; set; }
    }

    public interface ICameraModelData<TModel, TRevision>
        where TModel : ICameraModelData<TModel, TRevision>
        where TRevision : IRevisionData
    {
        IDictionary<string, TRevision> Revisions { get; set; }
    }
}
