using Microsoft.Extensions.Logging;
using Net.Chdk.Detectors.Camera;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Net.Chdk.Detectors.CameraModel
{
    sealed class DerivedCameraModelDetector : IOuterCameraModelDetector
    {
        private ILogger Logger { get; }
        private ICameraDetector CameraDetector { get; }
        private IEnumerable<IInnerCameraModelDetector> CameraModelDetectors { get; }

        public DerivedCameraModelDetector(IEnumerable<IInnerCameraModelDetector> cameraModelDetectors, ICameraDetector cameraDetector, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<DerivedCameraModelDetector>();
            CameraModelDetectors = cameraModelDetectors;
            CameraDetector = cameraDetector;
        }

        public CameraModels GetCameraModels(CardInfo cardInfo, SoftwareInfo softwareInfo, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting camera models from {0}", cardInfo.DriveLetter);

            var cameraInfo = CameraDetector.GetCamera(cardInfo, progress, token);
            if (cameraInfo == null)
                return null;

            var cameraModels = CameraModelDetectors
                .Select(d => d.GetCameraModels(cardInfo, cameraInfo, progress, token))
                .FirstOrDefault(c => c != null);

            return new CameraModels
            {
                Info = cameraInfo,
                Models = cameraModels.Collapse(cameraInfo)
            };
        }
    }
}
