using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Src;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class CameraProvider : CameraProvider<CameraData>
    {
        public CameraProvider(ILogger<CameraProvider> logger)
            : base(logger)
        {
        }

        protected override void UpdateValue(ref CameraData? camera, string line, string platform)
        {
            var split = line.Split();
            switch (split[0])
            {
                case "CAM_CLEAN_OVERLAY":
                    GetCamera(ref camera).CleanOverlay = GetBoolean(split, platform);
                    break;
            }
        }

        private static CameraData GetCamera(ref CameraData? camera)
        {
            return camera ??= new CameraData();
        }
    }
}
