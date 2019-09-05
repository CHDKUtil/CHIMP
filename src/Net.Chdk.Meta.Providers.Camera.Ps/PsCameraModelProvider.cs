using Net.Chdk.Meta.Model.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class PsCameraModelProvider : CameraModelProvider<PsCameraModelData>, IPsCameraModelProvider
    {
        public PsCameraModelProvider(ICameraModelValidator modelValidator, IRevisionProvider revisionProvider)
            : base(modelValidator, revisionProvider)
        {
        }
    }
}
