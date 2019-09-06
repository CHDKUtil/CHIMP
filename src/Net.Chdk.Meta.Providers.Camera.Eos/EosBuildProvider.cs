using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class EosBuildProvider : CategoryBuildProvider<EosCameraData, EosCameraModelData, EosCardData>
    {
        public EosBuildProvider(ICameraProvider<EosCameraData, EosCameraModelData, EosCardData> cameraProvider, ICameraModelProvider<EosCameraModelData> modelProvider,
            ICameraPlatformProvider platformProvider, ICameraValidator cameraValidator)
                : base(cameraProvider, modelProvider, platformProvider, cameraValidator)
        {
        }

        public override string CategoryName => "EOS";
    }
}
