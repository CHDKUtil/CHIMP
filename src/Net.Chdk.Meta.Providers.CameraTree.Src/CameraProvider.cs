using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Src;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
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
                case "CAM_ALT_BUTTON_NAMES":
                    GetCamera(ref camera).AltNames = GetAltButtonNames(split, platform);
                    break;
                case "CAM_MULTIPART":
                    GetCamera(ref camera).MultiCard = GetBoolean(split, platform);
                    break;
                default:
                    break;
            }
        }

        private static CameraData GetCamera(ref CameraData? camera)
        {
            return camera ??= new CameraData();
        }

        private string[] GetAltButtonNames(string[] split, string platform)
        {
            split = ParseArray(split, platform);

            for (int i = 0; i < split.Length; ++i)
                split[i] = TrimQuotes(split[i], platform);

            return split;
        }
    }
}
