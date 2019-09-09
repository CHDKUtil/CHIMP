using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class EosBuildProvider : CategoryBuildProvider<EosCameraData, EosCameraModelData, EosRevisionData, EosCardData>
    {
        public EosBuildProvider(ICameraProvider<EosCameraData, EosCameraModelData, EosRevisionData, EosCardData> cameraProvider, ICameraModelProvider<EosCameraModelData, EosRevisionData> modelProvider,
            ICameraPlatformProvider platformProvider, ICameraValidator cameraValidator)
                : base(cameraProvider, modelProvider, platformProvider, cameraValidator)
        {
        }

        public override string CategoryName => "EOS";
    }
}
