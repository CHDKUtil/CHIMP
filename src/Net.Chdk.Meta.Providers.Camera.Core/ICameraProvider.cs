using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    public interface ICameraProvider<TCamera, TModel, TRevision, TCard>
        where TCamera : CameraData<TCamera, TModel, TRevision, TCard>
        where TModel : CameraModelData<TModel, TRevision>
        where TRevision : IRevisionData
        where TCard : CardData
    {
        TCamera GetCamera(uint modelId, string platform, ListPlatformData list, TreePlatformData tree, string productName);
    }
}
