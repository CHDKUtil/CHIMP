using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Card;
using Net.Chdk.Validators;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.Camera
{
    [Obsolete]
    sealed class MetadataCameraDetector : MetadataDetector<MetadataCameraDetector, CameraInfo>, IInnerCameraDetector
    {
        public MetadataCameraDetector(IValidator<CameraInfo> validator, ILoggerFactory loggerFactory)
            : base(validator, loggerFactory)
        {
        }

        public CameraInfo GetCamera(CardInfo cardInfo, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting camera from {0} metadata", cardInfo.DriveLetter);

            return GetValue(cardInfo, progress, token);
        }

        protected override string FileName => Files.Metadata.Camera;
    }
}
