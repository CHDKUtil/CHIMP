using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Card;

namespace Net.Chdk.Detectors.Camera
{
    public abstract class FileSystemCameraDetectorBase : IInnerCameraDetector, IFilePatternProvider
    {
        private ILogger Logger { get; }
        private IFileCameraDetector FileCameraDetector { get; }

        protected FileSystemCameraDetectorBase(IFileCameraDetector fileCameraDetector, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<FileSystemCameraDetector>();
            FileCameraDetector = fileCameraDetector;
        }

        public abstract string[] Patterns { get; }
        public abstract string PatternsDescription { get; }

        public CameraInfo GetCamera(CardInfo cardInfo, IProgress<double> progress, CancellationToken token)
        {
            Logger.LogTrace("Detecting camera from {0} {1}", cardInfo.DriveLetter, PatternsDescription);

            var rootPath = cardInfo.GetRootPath();
            var path = Path.Combine(rootPath, Directories.Images);
            if (!Directory.Exists(path))
                return null;

            token.ThrowIfCancellationRequested();

            var dirs = Directory.EnumerateDirectories(path)
                .Reverse();
            var count = progress != null
                ? dirs.Count()
                : 0;
            var index = 0;

            foreach (var dir in dirs)
            {
                token.ThrowIfCancellationRequested();

                var camera = GetCameraFromDirectory(dir);
                if (IsValid(camera))
                    return camera;

                if (progress != null)
                    progress.Report((double)(++index) / count);
            }

            return null;
        }

        protected abstract bool IsValid(CameraInfo camera);

        private CameraInfo GetCameraFromDirectory(string dir)
        {
            return Patterns
                .Select(p => GetCameraFromDirectory(dir, p))
                .FirstOrDefault(c => IsValid(c));
        }

        private CameraInfo GetCameraFromDirectory(string dir, string pattern)
        {
            return Directory.EnumerateFiles(dir, pattern)
                .Reverse()
                .Select(GetCameraFromFile)
                .FirstOrDefault(IsValid);
        }

        private CameraInfo GetCameraFromFile(string file)
        {
            try
            {
                return FileCameraDetector.GetCamera(file);
            }
            catch (CameraDetectionException)
            {
                return null;
            }
        }
    }
}
