using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class EosBuildProvider : CategoryBuildProvider<EosCameraData, EosCardData>
    {
        public EosBuildProvider(ICameraProvider<EosCameraData, EosCardData> cameraProvider, ICameraModelProvider modelProvider,
            ICameraPlatformProvider platformProvider, ICameraValidator cameraValidator)
                : base(cameraProvider, modelProvider, platformProvider, cameraValidator)
        {
        }

        public override string CategoryName => "EOS";
    }
}
