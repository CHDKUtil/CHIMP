namespace Net.Chdk.Meta.Providers
{
    public interface ICameraMetaProvider
    {
        CameraInfo GetCamera(string productName, string fileName);
    }
}
