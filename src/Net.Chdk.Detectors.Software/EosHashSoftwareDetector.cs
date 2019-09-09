using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Chdk.Encoders.Binary;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;

namespace Net.Chdk.Detectors.Software
{
    sealed class EosHashSoftwareDetector : HashSoftwareDetector
    {
        public EosHashSoftwareDetector(IEnumerable<IProductBinarySoftwareDetector> softwareDetectors, IBinaryDecoder binaryDecoder, IBootProvider bootProvider, ICameraProvider cameraProvider, ISoftwareHashProvider hashProvider, IOptions<SoftwareDetectorSettings> settings, ILoggerFactory loggerFactory)
            : base(softwareDetectors, binaryDecoder, bootProvider, cameraProvider, hashProvider, settings, loggerFactory.CreateLogger<EosHashSoftwareDetector>())
        {
        }

        protected override string CategoryName => "EOS";
    }
}
