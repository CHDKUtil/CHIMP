using Net.Chdk.Meta.Model.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class PsBuildProvider : CategoryBuildProvider<PsCameraData, PsCameraModelData, RevisionData, PsCardData>
    {
        public PsBuildProvider(ICameraProvider<PsCameraData, PsCameraModelData, RevisionData, PsCardData> cameraProvider, ICameraModelProvider<PsCameraModelData, RevisionData> modelProvider,
            ICameraPlatformProvider platformProvider, ICameraValidator cameraValidator)
                : base(cameraProvider, modelProvider, platformProvider, cameraValidator)
        {
        }

        public override string CategoryName => "PS";
    }
}
