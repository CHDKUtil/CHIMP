using Net.Chdk.Meta.Model.Camera.Eos;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class EosCameraModelProvider : CameraModelProvider<EosCameraModelData>, IEosCameraModelProvider
    {
        public EosCameraModelProvider(ICameraModelValidator modelValidator, IRevisionProvider revisionProvider)
            : base(modelValidator, revisionProvider)
        {
        }
    }
}
