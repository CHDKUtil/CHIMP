using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Providers.Src;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class PlatformProvider : PlatformProvider<TreePlatformData, TreeRevisionData, CameraData, RevisionData, byte>
    {
        public PlatformProvider(RevisionProvider revisionProvider, CameraProvider cameraProvider, DataProvider dataProvider, ILogger<PlatformProvider> logger)
            : base(revisionProvider, cameraProvider, dataProvider, logger)
        {
        }

        protected override TreePlatformData GetPlatformData(string platformPath, string platform, string? sourcePlatform, IDictionary<string, TreeRevisionData> revisions, CameraData? camera)
        {
            return new TreePlatformData
            {
                Encoding = GetEncoding(platformPath, platform, sourcePlatform, revisions),
                AltNames = GetAltNames(camera),
                MultiCard = IsMultiCard(camera),
            };
        }

        protected override byte? GetValue(KeyValuePair<string, TreeRevisionData> kvp)
        {
            return kvp.Value.Encoding;
        }

        private byte GetEncoding(string platformPath, string platform, string? sourcePlatform, IDictionary<string, TreeRevisionData> revisions)
        {
            var data = GetData(platformPath, sourcePlatform, platform);
            return data?.Encoding
                ?? GetRevisionValue(revisions, platform)
                ?? 0;
        }

        private static string[]? GetAltNames(CameraData? camera)
        {
            return camera?.AltNames;
        }

        private static bool IsMultiCard(CameraData? camera)
        {
            return camera?.MultiCard == true;
        }
    }
}
