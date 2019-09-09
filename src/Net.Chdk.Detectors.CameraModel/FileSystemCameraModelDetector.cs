using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Card;
using Net.Chdk.Providers.Camera;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.CameraModel
{
    sealed class FileSystemCameraModelDetector : IInnerCameraModelDetector
    {
        private ILogger Logger { get; }
        private ICameraProvider CameraProvider { get; }

        public FileSystemCameraModelDetector(ICameraProvider cameraProvider, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<FileSystemCameraModelDetector>();
            CameraProvider = cameraProvider;
        }

        public CameraModelInfo[] GetCameraModels(CardInfo cardInfo, CameraInfo cameraInfo, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting camera models from camera info");

            var models = CameraProvider.GetCameraModels(cameraInfo);
            if (models == null)
                return null;

            return models.Models;
        }
    }
}
