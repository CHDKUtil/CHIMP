using Microsoft.Extensions.Logging;
using Net.Chdk.Detectors.Camera;
using Net.Chdk.Providers.Camera;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.CameraModel
{
    public sealed class FileCameraModelDetector : IFileCameraModelDetector
    {
        private ILogger Logger { get; }
        private IFileCameraDetector FileCameraDetector { get; }
        private ICameraProvider CameraProvider { get; }

        public FileCameraModelDetector(IFileCameraDetector fileCameraDetector, ICameraProvider cameraProvider, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<FileCameraModelDetector>();
            FileCameraDetector = fileCameraDetector;
            CameraProvider = cameraProvider;
        }

        public CameraModels GetCameraModels(string filePath, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting camera models from {0}", filePath);

            var cameraInfo = FileCameraDetector.GetCamera(filePath);
            if (cameraInfo == null)
                return null;

            var cameraModels = CameraProvider.GetCameraModels(cameraInfo);

            return new CameraModels
            {
                Info = cameraInfo,
                Models = cameraModels?.Models.Collapse(cameraInfo)
            };
        }
    }
}
