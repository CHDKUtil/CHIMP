using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
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
            return canon != null
                ? canon.FirmwareRevision != 0
                    ? "PS"
                    : "EOS"
                : null;
        }

        public string? GetFirmwareRevision(CameraInfo? cameraInfo)
        {
            var categoryName = GetCategoryName(cameraInfo);
            return GetProvider(categoryName)?.GetFirmwareRevision(cameraInfo);
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
