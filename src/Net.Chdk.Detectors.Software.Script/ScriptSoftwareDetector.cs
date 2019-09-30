using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Chdk.Encoders.Binary;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Software;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Net.Chdk.Detectors.Software.Script
{
    sealed class ScriptSoftwareDetector : BinarySoftwareDetectorBase
    {
        private static readonly Version Version = new Version("1.0");

        public ScriptSoftwareDetector(IEnumerable<IProductBinarySoftwareDetector> softwareDetectors, IBinaryDecoder binaryDecoder, IBootProvider bootProvider, ICameraProvider cameraProvider, ISoftwareHashProvider hashProvider, IOptions<SoftwareDetectorSettings> settings, ILogger<ScriptSoftwareDetector> logger)
            : base(softwareDetectors, binaryDecoder, bootProvider, cameraProvider, hashProvider, settings, logger)
        {
        }

        public override bool UpdateSoftware(SoftwareInfo software, byte[] inBuffer)
        {
            if (!CategoryName.Equals(software.Category.Name, StringComparison.Ordinal))
                return false;

            var software2 = GetSoftware(inBuffer, null, default);
            if (software2 != null)
            {
                software.Product = software2.Product;
                software.Build = software2.Build;
                software.Camera = software2.Camera;
                software.Hash = software2.Hash;
                return true;
            }

            return false;
        }

        protected override SoftwareInfo? DoGetSoftware(byte[] inBuffer, IProgress<double>? progress, CancellationToken token)
        {
            var text = Encoding.ASCII.GetString(inBuffer);
            var lines = text.Split('\n');
            SoftwareInfo? software = null;
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].TrimStart();
                if (!string.IsNullOrEmpty(line))
                {
                    if (line[0] != '\'')
                        break;
                    line = line.Substring(1).TrimStart().TrimEnd('\r');
                    if (line.Length > 0 && line[0] == '@')
                        UpdateSoftware(ref software, line);
                }
            }
            return software;
        }

        protected override uint?[] GetOffsets()
        {
            return Array.Empty<uint?>();
        }

        protected override string CategoryName => "SCRIPT";

        private void UpdateSoftware(ref SoftwareInfo? software, string line)
        {
            var split = line.Substring(1).Split(' ');
            if (split.Length == 2)
                UpdateSoftware(ref software, split[0], split[1].Trim());
        }

        private void UpdateSoftware(ref SoftwareInfo? software, string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
                switch (key)
                {
                    case "script":
                        var split = value.Split('-');
                        UpdateProduct(ref software, split);
                        UpdateCamera(ref software, split);
                        UpdateBuild(ref software, split);
                        break;
                    case "author":
                        UpdateAuthor(ref software, value);
                        break;
                    case "created":
                        UpdateCreated(ref software, value);
                        break;
                }
        }

        private void UpdateProduct(ref SoftwareInfo? software, string[] split)
        {
            var product = GetProduct(ref software);
            product.Name = split[0];
            if (split.Length > 1)
                product.Version = Version.Parse(split[1]);
        }

        private void UpdateCamera(ref SoftwareInfo? software, string[] split)
        {
            var camera = GetCamera(ref software);
            if (split.Length > 3)
            {
                camera.Platform = split[2];
                camera.Revision = split[3];
            }
        }

        private void UpdateBuild(ref SoftwareInfo? software, string[] split)
        {
            var build = GetBuild(ref software);
            if (split.Length > 4)
            {
                split = split[4].Split('_');
                build.Status = split.Length > 1
                    ? split[1]
                    : split[0];
                if (split.Length > 1)
                    build.Name = split[0];
            }
        }

        private void UpdateAuthor(ref SoftwareInfo? software, string value)
        {
            GetBuild(ref software).Creator = value;
        }

        private void UpdateCreated(ref SoftwareInfo? software, string value)
        {
            if (DateTime.TryParse(value, out DateTime created))
                GetProduct(ref software).Created = created;
        }

        private SoftwareProductInfo GetProduct(ref SoftwareInfo? software)
        {
            return GetSoftware(ref software).Product ??= new SoftwareProductInfo
            {
                Language = CultureInfo.InvariantCulture
            };
        }

        private SoftwareBuildInfo GetBuild(ref SoftwareInfo? software)
        {
            return GetSoftware(ref software).Build ??= new SoftwareBuildInfo
            {
                Name = string.Empty,
                Status = string.Empty
            };
        }

        private SoftwareCameraInfo GetCamera(ref SoftwareInfo? software)
        {
            return GetSoftware(ref software).Camera ??= new SoftwareCameraInfo();
        }

        private SoftwareInfo GetSoftware(ref SoftwareInfo? software)
        {
            return software ??= new SoftwareInfo
            {
                Version = Version,
                Category = new CategoryInfo
                {
                    Name = CategoryName
                }
            };
        }
    }
}
