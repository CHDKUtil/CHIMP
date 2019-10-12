using Microsoft.Extensions.Logging;

namespace Net.Chdk.Meta.Providers.Src
{
    public abstract class CameraProvider<TCamera> : HeaderParsingProvider<TCamera>
        where TCamera : class
    {
        protected CameraProvider(ILogger logger)
            : base(logger)
        {
        }

        public TCamera? GetCamera(string platformPath, string platform)
        {
            return GetValue(platformPath, platform, null);
        }

        protected override string FileName => "platform_camera.h";
    }
}
