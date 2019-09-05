using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class EosBuildProvider : CategoryBuildProvider<EosCameraData, EosCameraModelData, EosCardData>
    {
        public EosBuildProvider(IEosCameraProvider cameraProvider, IEosCameraModelProvider modelProvider, ICameraPlatformProvider platformProvider, ICameraValidator cameraValidator)
            : base(cameraProvider, modelProvider, platformProvider, cameraValidator)
        {
        }

        public override string CategoryName => "EOS";
    }
}
