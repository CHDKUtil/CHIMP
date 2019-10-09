using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Firmware
{
    sealed class FirmwareProvider : ProviderResolver<IInnerFirmwareProvider>, IFirmwareProvider
    {
        public FirmwareProvider(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }

        public string? GetCategoryName(CameraInfo? cameraInfo)
        {
            var canon = cameraInfo?.Canon;
            if (canon == null)
                return null;
            if (canon.FirmwareRevision != 0)
                return "PS";
            if (canon.FirmwareVersion != null)
                return "EOS";
            return null;
        }

        public string? GetCategoryName(SoftwareCameraInfo? camera)
        {
            if (camera?.Revision == null)
                return null;
            return camera.Revision.Length switch
            {
                3 => "EOS",
                4 => "PS",
                _ => null,
            };
        }

        public string? GetFirmwareRevision(CameraInfo? cameraInfo, string? categoryName = null)
        {
            categoryName ??= GetCategoryName(cameraInfo);
            return GetProvider(categoryName)?
                .GetFirmwareRevision(cameraInfo);
        }

        protected override IEnumerable<string> GetNames()
        {
            return new[] { "EOS", "PS" };
        }

        protected override IInnerFirmwareProvider CreateProvider(string categoryName) => categoryName switch
        {
            "EOS" => new EosFirmwareProvider(),
            "PS" => new PsFirmwareProvider(),
            _ => throw new InvalidOperationException($"Unknown category: {categoryName}"),
        };
    }
}
