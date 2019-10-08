using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Chdk.Encoders.Binary;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Software;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    sealed class UnknownPsBinarySoftwareDetector : PsBinarySoftwareDetector
    {
        public UnknownPsBinarySoftwareDetector(IEnumerable<IProductBinarySoftwareDetector> softwareDetectors, IBinaryDecoder binaryDecoder, IBootProvider bootProvider, ICameraProvider cameraProvider, ISoftwareHashProvider hashProvider, IOptions<SoftwareDetectorSettings> settings, ILoggerFactory loggerFactory)
            : base(softwareDetectors, binaryDecoder, bootProvider, cameraProvider, hashProvider, settings, loggerFactory.CreateLogger<UnknownPsBinarySoftwareDetector>())
        {
        }

        protected override SoftwareInfo DoGetSoftware(IEnumerable<IProductBinarySoftwareDetector> detectors, byte[] prefix, byte[] inBuffer, IProgress<double> progress, CancellationToken token)
        {
            return PlainGetSoftware(detectors, prefix, inBuffer, token)
                ?? base.DoGetSoftware(detectors, prefix, inBuffer, progress, token);
        }

        protected override uint?[] GetOffsets()
        {
            var offsetCount = Offsets.GetOffsetCount();
            var offsets = new uint?[offsetCount];
            var index = 0;
            Offsets.Empty.GetAllOffsets(offsets, ref index);
            if (ShuffleOffsets)
                Shuffle(offsets);
            return offsets;
        }

        private static void Shuffle(uint?[] offsets)
        {
            var random = new Random(DateTime.Now.Millisecond);
            for (var i = 0; i < offsets.Length; i++)
            {
                var j = random.Next(offsets.Length);
                var tmp = offsets[i];
                offsets[i] = offsets[j];
                offsets[j] = tmp;
            }
        }
    }
}
