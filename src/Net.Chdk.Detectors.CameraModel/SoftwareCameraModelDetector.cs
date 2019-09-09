using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Camera;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.CameraModel
{
    sealed class SoftwareCameraModelDetector : IOuterCameraModelDetector
    {
        private ILogger Logger { get; }
        private ICameraProvider CameraProvider { get; }

        public SoftwareCameraModelDetector(ICameraProvider cameraProvider, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<SoftwareCameraModelDetector>();
            CameraProvider = cameraProvider;
        }

        public CameraModels GetCameraModels(CardInfo cardInfo, SoftwareInfo softwareInfo, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting camera models from {0} software", cardInfo.DriveLetter);

            if (softwareInfo == null)
                return null;

            var modelsInfo = CameraProvider.GetCameraModels(softwareInfo.Product, softwareInfo.Camera);
            if (modelsInfo == null)
                return null;

            return new CameraModels
            {
                Info = modelsInfo.Info,
                Models = modelsInfo.Models,
            };
        }
    }
}
