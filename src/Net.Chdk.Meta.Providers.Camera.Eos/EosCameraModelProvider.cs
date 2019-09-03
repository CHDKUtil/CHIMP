using Net.Chdk.Meta.Model.Camera.Eos;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class EosCameraModelProvider : CameraModelProvider<EosCameraModelData>, IEosCameraModelProvider
    {
        private IVersionProvider VersionProvider { get; }

        public EosCameraModelProvider(IVersionProvider versionProvider, ICameraModelValidator modelValidator)
            : base(modelValidator)
        {
            VersionProvider = versionProvider;
        }

        public override EosCameraModelData GetModel(string platform, string[] names, ListPlatformData list, TreePlatformData tree, string productName)
        {
            var model = base.GetModel(platform, names, list, tree, productName);
            model.Versions = VersionProvider.GetVersions(productName, list, tree);
            return model;
        }
    }
}
