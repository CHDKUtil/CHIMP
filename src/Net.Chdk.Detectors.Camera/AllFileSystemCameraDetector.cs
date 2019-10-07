using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;

namespace Net.Chdk.Detectors.Camera
{
    sealed class AllFileSystemCameraDetector : FileSystemCameraDetectorBase
    {
        public AllFileSystemCameraDetector(IFileCameraDetector fileCameraDetector, ILoggerFactory loggerFactory)
            : base(fileCameraDetector, loggerFactory)
        {
        }

        public override string[] Patterns => new[]
        {
            "*.JPG",
            "*.THM",
            "*.CR2",
            "*.CR3",
            "*.CRM",
        };

        public override string PatternsDescription => "CompatibleImages";

        protected override bool IsValid(CameraInfo? camera) => camera?.Canon != null;
    }
}
