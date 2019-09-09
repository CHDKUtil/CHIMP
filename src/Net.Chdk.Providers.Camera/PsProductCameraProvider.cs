using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Camera
{
    sealed class PsProductCameraProvider : ProductCameraProvider<PsCameraData, PsCameraModelData, PsCardData, PsReverseCameraData, RevisionData, uint>
    {
        public PsProductCameraProvider(string productName, ILoggerFactory loggerFactory)
            : base(productName, loggerFactory.CreateLogger<PsProductCameraProvider>())
        {
        }

        protected override string GetRevision(CameraInfo cameraInfo, PsCameraModelData model)
        {
            var revisionStr = $"0x{cameraInfo.Canon.FirmwareRevision:x}";
            return model.Revisions.TryGetValue(revisionStr, out RevisionData revision)
                ? revision.Revision
                : null;
        }

        protected override bool IsInvalid(CameraInfo cameraInfo)
        {
            return cameraInfo.Canon?.ModelId == null || cameraInfo.Canon?.FirmwareRevision == 0;
        }

        protected override CanonInfo CreateCanonInfo(PsReverseCameraData camera, uint revision)
        {
            return new CanonInfo
            {
                ModelId = camera.ModelId,
                FirmwareRevision = revision
            };
        }

        protected override bool GetCamera(PsReverseCameraData reverse, SoftwareCameraInfo camera, out uint revision)
        {
            return reverse.Revisions.TryGetValue(camera.Revision, out revision);
        }

        protected override PsReverseCameraData CreateReverseCamera(string key, PsCameraData camera, PsCameraModelData model)
        {
            var reverse = base.CreateReverseCamera(key, camera, model);
            reverse.Revisions = GetRevisions(model);
            return reverse;
        }

        protected override CameraModelsInfo GetCameraModels(PsCameraData camera, CameraModelInfo[] models)
        {
            var cameraModels = base.GetCameraModels(camera, models);
            cameraModels.IsMultiPartition = camera.Card?.Multi == true;
            return cameraModels;
        }

        protected override SoftwareEncodingInfo GetEncoding(PsCameraData camera)
        {
            return new SoftwareEncodingInfo
            {
                Name = camera.Encoding.Name,
                Data = camera.Encoding.Data,
            };
        }

        protected override AltInfo GetAlt(PsCameraData camera)
        {
            return new AltInfo
            {
                Button = camera.Alt.Button,
                Buttons = camera.Alt.Buttons,
            };
        }

        private static Dictionary<string, uint> GetRevisions(PsCameraModelData model)
        {
            return model.Revisions.ToDictionary(GetKey, GetValue);
        }

        private static string GetKey(KeyValuePair<string, RevisionData> kvp)
        {
            var revision = GetValue(kvp);
            return GetFirmwareRevision(revision);
        }

        private static uint GetValue(KeyValuePair<string, RevisionData> kvp)
        {
            return Convert.ToUInt32(kvp.Key, 16);
        }

        private static string GetFirmwareRevision(uint revision)
        {
            return new string(new[] {
                (char)(((revision >> 24) & 0x0f) + 0x30),
                (char)(((revision >> 20) & 0x0f) + 0x30),
                (char)(((revision >> 16) & 0x0f) + 0x30),
                (char)(((revision >>  8) & 0x7f) + 0x60)
            });
        }
    }
}
