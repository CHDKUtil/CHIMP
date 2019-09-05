using System.Collections.Generic;

namespace Net.Chdk.Meta.Model.Camera
{
    public interface ICameraModelData
    {
        string[] Names { get; }
        string Platform { get; }
        IDictionary<string, IRevisionData> Revisions { get; set; }
    }
}
