using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class EosBuildProvider : CategoryBuildProvider<EosCameraData, EosCameraModelData, VersionData, EosCardData>
    {
        public EosBuildProvider(ICameraProvider<EosCameraData, EosCameraModelData, VersionData, EosCardData> cameraProvider, ICameraModelProvider<EosCameraModelData, VersionData> modelProvider,
            ICameraPlatformProvider platformProvider, ICameraValidator cameraValidator)
                : base(cameraProvider, modelProvider, platformProvider, cameraValidator)
        {
        }

        public override string CategoryName => "EOS";
    }
}
