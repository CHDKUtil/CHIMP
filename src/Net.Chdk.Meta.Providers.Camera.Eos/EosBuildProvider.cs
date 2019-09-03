using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class EosBuildProvider : BuildProvider<EosCameraData, EosCameraModelData, EosCardData>, IEosBuildProvider
    {
        public EosBuildProvider(IEosCameraProvider cameraProvider, IEosCameraModelProvider modelProvider, ICameraPlatformProvider platformProvider, ICameraValidator cameraValidator)
            : base(cameraProvider, modelProvider, platformProvider, cameraValidator)
        {
        }
    }
}
