using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
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

        public string? GetFirmwareRevision(CameraInfo? cameraInfo, string? categoryName = null)
        {
            categoryName ??= GetCategoryName(cameraInfo);
            return GetProvider(categoryName)?
                .GetFirmwareRevision(cameraInfo);
        }

        public string? GetRevisionString(string revision, string categoryName)
        {
            return GetProvider(categoryName)?
                .GetRevisionString(revision);
        }

        public string? GetModelName(CameraInfo camera, CameraModelInfo cameraModel)
        {
            return cameraModel.Names.Length == 1
                ? cameraModel.Names[0]
                : camera.Base.Model;
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
