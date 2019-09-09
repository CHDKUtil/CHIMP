using Net.Chdk.Meta.Model.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class PsBuildProvider : CategoryBuildProvider<PsCameraData, PsCameraModelData, PsRevisionData, PsCardData>
    {
        public PsBuildProvider(ICameraProvider<PsCameraData, PsCameraModelData, PsRevisionData, PsCardData> cameraProvider, ICameraModelProvider<PsCameraModelData, PsRevisionData> modelProvider,
            ICameraPlatformProvider platformProvider, ICameraValidator cameraValidator)
                : base(cameraProvider, modelProvider, platformProvider, cameraValidator)
        {
        }

        public override string CategoryName => "PS";
    }
}
