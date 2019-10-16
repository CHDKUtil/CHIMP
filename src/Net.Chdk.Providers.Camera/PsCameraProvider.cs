using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Camera
{
    sealed class PsCameraProvider : ProductCameraProvider<PsCameraData, PsCardData>
    {
        public PsCameraProvider(string productName, ILoggerFactory loggerFactory)
            : base(productName, loggerFactory.CreateLogger<PsCameraProvider>())
        {
            _reverseCameras = new Lazy<Dictionary<string, ReverseCameraData>>(GetReverseCameras);
        }

        public override SoftwareEncodingInfo? GetEncoding(SoftwareCameraInfo cameraInfo)
        {
            if (cameraInfo?.Platform == null)
                return null;
            ReverseCameras.TryGetValue(cameraInfo.Platform, out ReverseCameraData? camera);
            return GetEncoding(camera?.EncodingData);
        }

        public override string? GetAltButton(SoftwareCameraInfo cameraInfo)
        {
            if (cameraInfo?.Platform == null)
                return null;
            ReverseCameras.TryGetValue(cameraInfo.Platform, out ReverseCameraData? camera);
            return camera?.AltButton;
        }

        protected override bool IsInvalid(CameraInfo cameraInfo)
        {
            return cameraInfo?.Canon?.ModelId == null || cameraInfo?.Canon?.FirmwareRevision == 0;
        }

        protected override bool IsMultiPartition(PsCameraData? camera)
        {
            return camera?.Card?.Multi == true;
        }

        #region ReverseCameras

        private readonly Lazy<Dictionary<string, ReverseCameraData>> _reverseCameras;

        private Dictionary<string, ReverseCameraData> ReverseCameras => _reverseCameras.Value;

        private Dictionary<string, ReverseCameraData> GetReverseCameras()
        {
            return Data
                .SelectMany(GetReverseCameras)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private IEnumerable<KeyValuePair<string, ReverseCameraData>> GetReverseCameras(KeyValuePair<string, PsCameraData> kvp)
        {
            return kvp.Value.Models.Select(model => GetReverseCamera(kvp.Value, model.Platform));
        }

        private KeyValuePair<string, ReverseCameraData> GetReverseCamera(PsCameraData camera, string? platform)
        {
            var reverse = new ReverseCameraData
            {
                EncodingData = camera.Encoding?.Data,
                AltButton = camera.Alt?.Button,
            };
            return new KeyValuePair<string, ReverseCameraData>(platform!, reverse);
        }

        #endregion

        private static SoftwareEncodingInfo? GetEncoding(uint? data)
        {
            if (data == null)
                return null;

            if (data.Value == 0)
                return SoftwareEncodingInfo.Empty;

            return new SoftwareEncodingInfo
            {
                Name = "dancingbits",
                Data = data,
            };
        }
    }
}
