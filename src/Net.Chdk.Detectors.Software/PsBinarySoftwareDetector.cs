using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Chdk.Encoders.Binary;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;

namespace Net.Chdk.Detectors.Software
{
    abstract class PsBinarySoftwareDetector : BinarySoftwareDetectorBase
    {
        protected PsBinarySoftwareDetector(IEnumerable<IProductBinarySoftwareDetector> softwareDetectors, IBinaryDecoder binaryDecoder, IBootProvider bootProvider, ICameraProvider cameraProvider, ISoftwareHashProvider hashProvider, IOptions<SoftwareDetectorSettings> settings, ILogger logger)
            : base(softwareDetectors, binaryDecoder, bootProvider, cameraProvider, hashProvider, settings, logger)
        {
        }

        protected override string CategoryName => "PS";
    }
}
