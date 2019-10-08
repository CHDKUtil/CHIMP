using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.CameraModel;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.CameraModel
{
    sealed class SoftwareCameraModelDetector : IOuterCameraModelDetector
    {
        private ILogger Logger { get; }
        private ICameraModelProvider CameraProvider { get; }

        public SoftwareCameraModelDetector(ICameraModelProvider cameraProvider, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<SoftwareCameraModelDetector>();
            CameraProvider = cameraProvider;
        }

        public (CameraInfo, CameraModelInfo[])? GetCameraModels(CardInfo cardInfo, SoftwareInfo? softwareInfo, IProgress<double>? progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting camera models from {0} software", cardInfo.DriveLetter);

            if (softwareInfo == null)
                return null;

            return CameraProvider.GetCameraModels(softwareInfo.Camera, softwareInfo.Model)
                ?? CameraProvider.GetCameraModels(softwareInfo.Product, softwareInfo.Camera);
        }
    }
}
