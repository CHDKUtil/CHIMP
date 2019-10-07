using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Exif.Makernotes;
using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using System;
using System.IO;
using System.Linq;

using MetadataCollection = System.Collections.Generic.IReadOnlyList<MetadataExtractor.Directory>;

namespace Net.Chdk.Detectors.Camera
{
    sealed class FileCameraDetector : IFileCameraDetector
    {
        private ILogger Logger { get; }

        public FileCameraDetector(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<FileCameraDetector>();
        }

        public CameraInfo? GetCamera(string filePath)
        {
            Logger.LogInformation("Reading {0}", filePath);

            using (var stream = File.OpenRead(filePath))
            {
                return GetCamera(stream);
            }
        }

        private CameraInfo? GetCamera(Stream stream)
        {
            try
            {
                var metadata = ImageMetadataReader.ReadMetadata(stream);
                if (metadata.Count == 0)
                    return null;

                return new CameraInfo
                {
                    Base = GetBase(metadata),
                    Canon = GetCanon(metadata),
                };
            }
            catch (MetadataException ex)
            {
                Logger.LogError(0, ex, "Error reading metadata");
                throw new CameraDetectionException(ex);
            }
            catch (ImageProcessingException ex)
            {
                Logger.LogError(0, ex, "Error processing metadata");
                throw new CameraDetectionException(ex);
            }
        }

        private static BaseInfo? GetBase(MetadataCollection metadata)
        {
            var ifd0 = metadata.OfType<ExifIfd0Directory>().SingleOrDefault();
            if (ifd0 == null)
                return null;

            return new BaseInfo
            {
                Make = ifd0.GetString(ExifDirectoryBase.TagMake),
                Model = ifd0.GetString(ExifDirectoryBase.TagModel),
            };
        }

        private static CanonInfo? GetCanon(MetadataCollection metadata)
        {
            var canon = metadata.OfType<CanonMakernoteDirectory>().SingleOrDefault();
            if (canon == null)
                return null;

            canon.TryGetUInt32(CanonMakernoteDirectory.TagModelId, out uint modelId);

            Version? firmwareVersion = !canon.TryGetUInt32(CanonMakernoteDirectory.TagFirmwareRevision, out uint firmwareRevision)
                ? GetFirmwareVersion(canon)
                : null;
            return new CanonInfo
            {
                ModelId = modelId,
                FirmwareRevision = firmwareRevision,
                FirmwareVersion = firmwareVersion
            };
        }

        private static Version? GetFirmwareVersion(CanonMakernoteDirectory canon)
        {
            var str = canon.GetString(CanonMakernoteDirectory.TagCanonFirmwareVersion);
            if (str == null)
                return null;

            str = str.TrimStart("Firmware Version ");
            if (str == null)
                return null;

            Version.TryParse(str, out Version firmwareVersion);
            return firmwareVersion;
        }
    }
}
