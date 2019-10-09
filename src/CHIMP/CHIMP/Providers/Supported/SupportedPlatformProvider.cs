using Chimp.Model;
using Chimp.Properties;
using Net.Chdk;
using Net.Chdk.Model.CameraModel;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.CameraModel;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Supported
{
    sealed class SupportedPlatformProvider : IInnerSupportedProvider
    {
        private ICameraModelProvider CameraProvider { get; }

        public SupportedPlatformProvider(ICameraModelProvider cameraProvider)
        {
            CameraProvider = cameraProvider;
        }

        public bool IsMatch(MatchData data)
        {
            return data.Platforms != null;
        }

        public string GetError(MatchData data)
        {
            return Resources.Download_UnsupportedModel_Text;
        }

        public string[] GetItems(MatchData data, SoftwareInfo software)
        {
            return data.Platforms
                .SelectMany(p => GetModels(p, software))
                .ToArray();
        }

        public string GetTitle(MatchData data)
        {
            return data.Platforms.Count() > 1
                ? Resources.Download_SupportedModels_Content
                : Resources.Download_SupportedModel_Content;
        }

        private IEnumerable<string> GetModels(string platform, SoftwareInfo software)
        {
            var camera = GetCamera(platform, software.Camera?.Revision);
            var data = CameraProvider.GetCameraModels(software.Product, camera);
            if (data?.Models != null)
                foreach (var model in data?.Models)
                    yield return GetModel(model);
        }

        private SoftwareCameraInfo GetCamera(string platform, string revision)
        {
            return new SoftwareCameraInfo
            {
                Platform = platform,
                Revision = revision
            };
        }

        private static string GetModel(CameraModelInfo model)
        {
            var models = Enumerable.Range(0, model.Names.Length)
                .Select(i => GetModel(model.Names, i));
            return string.Join(" / ", models);
        }

        private static string GetModel(string[] models, int index)
        {
            var model = models[index]
                .TrimStart("Canon ")
                .TrimEnd(" IS")
                .TrimEnd(" HS")
                .TrimEnd(" Ti");

            if (index > 0)
                model = model
                    .TrimStart("EOS ")
                    .TrimStart("PowerShot ");

            return model;
        }
    }
}
