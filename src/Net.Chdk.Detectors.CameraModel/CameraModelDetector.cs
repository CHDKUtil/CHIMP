using Net.Chdk.Model.Card;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Net.Chdk.Detectors.CameraModel
{
    public sealed class CameraModelDetector : ICameraModelDetector
    {
        private IEnumerable<IOuterCameraModelDetector> CameraModelDetectors { get; }

        public CameraModelDetector(IEnumerable<IOuterCameraModelDetector> cameraModelDetectors)
        {
            CameraModelDetectors = cameraModelDetectors;
        }

        public CameraModels GetCameraModels(CardInfo cardInfo, SoftwareInfo softwareInfo, IProgress<double> progress, CancellationToken token)
        {
            return CameraModelDetectors
                .Select(d => d.GetCameraModels(cardInfo, softwareInfo, progress, token))
                .FirstOrDefault(c => c != null);
        }
    }
}
