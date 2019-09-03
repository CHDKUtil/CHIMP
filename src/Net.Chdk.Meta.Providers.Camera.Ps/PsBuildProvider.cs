using Net.Chdk.Meta.Model.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class PsBuildProvider : BuildProvider<PsCameraData, PsCameraModelData, PsCardData>, IPsBuildProvider
    {
        public PsBuildProvider(IPsCameraProvider cameraProvider, IPsCameraModelProvider modelProvider, ICameraPlatformProvider platformProvider, ICameraValidator cameraValidator)
            : base(cameraProvider, modelProvider, platformProvider, cameraValidator)
        {
        }
    }
}
