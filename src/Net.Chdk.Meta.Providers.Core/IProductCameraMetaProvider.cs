namespace Net.Chdk.Meta.Providers
{
    public interface IProductCameraMetaProvider : IProductNameProvider
    {
        CameraInfo GetCamera(string fileName);
    }
}
