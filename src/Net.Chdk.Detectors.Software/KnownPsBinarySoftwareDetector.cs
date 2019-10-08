using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Chdk.Encoders.Binary;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;

namespace Net.Chdk.Detectors.Software
{
    sealed class KnownPsBinarySoftwareDetector : PsBinarySoftwareDetector
    {
        private int[][] Offsets { get; }

        public KnownPsBinarySoftwareDetector(IEnumerable<IProductBinarySoftwareDetector> softwareDetectors, IBinaryDecoder binaryDecoder, IBootProvider bootProvider, ICameraProvider cameraProvider, ISoftwareHashProvider hashProvider, IOptions<SoftwareDetectorSettings> settings, ILoggerFactory loggerFactory)
            : base(softwareDetectors, binaryDecoder, bootProvider, cameraProvider, hashProvider, settings, loggerFactory.CreateLogger<KnownPsBinarySoftwareDetector>())
        {
            Offsets = bootProvider.GetOffsets(CategoryName);
        }

        protected override uint?[] GetOffsets()
        {
            var offsets = new uint?[Offsets.Length + 1];
            for (var v = 0; v < Offsets.Length; v++)
                offsets[v + 1] = GetOffsets(v + 1);
            return offsets;
        }

        private uint? GetOffsets(int version)
        {
            var offsets = Offsets[version - 1];
            return GetOffsets(offsets);
        }

        private static uint? GetOffsets(int[] offsets)
        {
            var uOffsets = 0u;
            for (int index = 0; index < offsets.Length; index++)
                uOffsets += (uint)offsets[index] << (index << 2);
            return uOffsets;
        }
    }
}
