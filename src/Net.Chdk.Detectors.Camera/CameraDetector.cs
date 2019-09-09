using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Net.Chdk.Detectors.Camera
{
    public sealed class CameraDetector : ICameraDetector
    {
        private ILogger Logger { get; }
        private IEnumerable<IInnerCameraDetector> CameraDetectors { get; }

        public CameraDetector(IEnumerable<IInnerCameraDetector> cameraDetectors, IFileCameraDetector fileCameraDetector, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<CameraDetector>();
            CameraDetectors = cameraDetectors;
        }

        public CameraInfo GetCamera(CardInfo cardInfo, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting camera from {0}", cardInfo.DriveLetter);

            return CameraDetectors
                .Select(d => d.GetCamera(cardInfo, progress, token))
                .FirstOrDefault(c => c != null);
        }
    }
}
