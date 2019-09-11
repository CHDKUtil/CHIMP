using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    sealed class CameraModelProvider : ICameraModelProvider
    {
        private ICameraModelValidator ModelValidator { get; }

        public CameraModelProvider(ICameraModelValidator modelValidator)
        {
            ModelValidator = modelValidator;
        }

        public CameraModelData GetModel(string platform, string[] names, ListPlatformData list, TreePlatformData tree, string productName)
        {
            ModelValidator.Validate(platform, list, tree, productName);
            return new CameraModelData
            {
                Platform = platform,
                Names = names,
            };
        }
    }
}
