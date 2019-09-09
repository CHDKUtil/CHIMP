using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;

namespace Net.Chdk.Detectors.Camera
{
    sealed class FileSystemCameraDetector : FileSystemCameraDetectorBase
    {
        public FileSystemCameraDetector(IFileCameraDetector fileCameraDetector, ILoggerFactory loggerFactory)
            : base(fileCameraDetector, loggerFactory)
        {
        }

        public override string[] Patterns => new[]
        {
            "IMG_????.JPG",
            "_MG_????.JPG",
            "MVI_????.THM",
            "IMG_????.CR2",
            "_MG_????.CR2",
            "IMG_????.CR3",
            "_MG_????.CR3",
            "MVI_????.CRM",
            "_VI_????.CRM",
        };

        public override string PatternsDescription => "CanonImages";

        protected override bool IsValid(CameraInfo camera) => camera != null;
    }
}
