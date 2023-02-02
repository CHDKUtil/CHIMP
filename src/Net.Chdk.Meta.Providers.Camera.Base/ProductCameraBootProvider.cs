using Net.Chdk.Meta.Model.Camera;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class ProductCameraBootProvider : IProductCameraBootProvider
    {
        public BootData GetBoot(uint modelId, bool multi)
        {
            return new BootData
            {
                Fs = GetBootFileSystem(modelId, multi),
            };
        }

        public abstract string ProductName { get; }

        protected abstract string GetBootFileSystem(uint modelId, bool multi);
    }
}
