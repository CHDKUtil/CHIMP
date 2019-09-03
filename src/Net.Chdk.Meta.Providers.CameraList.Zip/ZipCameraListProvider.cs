using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Providers.Zip;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Product;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraList.Zip
{
    sealed class ZipCameraListProvider : ZipMetaProvider<CameraInfo>, IInnerCameraListProvider
    {
        private ICameraMetaProvider CameraProvider { get; }

        public ZipCameraListProvider(ICameraMetaProvider cameraProvider, IProductProvider productProvider, IBootProvider bootProvider, ILogger<ZipCameraListProvider> logger)
            : base(productProvider, bootProvider, logger)
        {
            CameraProvider = cameraProvider;
        }

        public IDictionary<string, ListPlatformData> GetCameraList(string path, string productName)
        {
            var cameraList = new SortedDictionary<string, ListPlatformData>();
            var cameras = GetItems(path, productName);
            foreach (var camera in cameras)
            {
                if (camera != null)
                {
                    AddCamera(cameraList, camera.Platform, camera.Revision, null);
                }
            }
            return cameraList;
        }

        private static void AddCamera(IDictionary<string, ListPlatformData> cameras, string platform, string revision, string source)
        {
            var platformData = GetOrAddPlatform(cameras, platform);
            var revisionData = GetRevisionData(platform, revision, source);
            platformData.Revisions.Add(revision, revisionData);
        }

        private static ListPlatformData GetOrAddPlatform(IDictionary<string, ListPlatformData> cameras, string platform)
        {
            ListPlatformData platformData;
            if (!cameras.TryGetValue(platform, out platformData))
            {
                platformData = GetPlatformData();
                cameras.Add(platform, platformData);
            }
            return platformData;
        }

        private static ListPlatformData GetPlatformData()
        {
            return new ListPlatformData
            {
                Revisions = new SortedDictionary<string, ListRevisionData>()
            };
        }

        private static ListRevisionData GetRevisionData(string platform, string revision, string source)
        {
            return new ListRevisionData
            {
                Source = GetSourceData(platform, revision, source)
            };
        }

        private static ListSourceData GetSourceData(string platform, string revision, string source)
        {
            return new ListSourceData
            {
                Revision = GetRevision(revision, source)
            };
        }

        private static string GetRevision(string revision, string source)
        {
            if (!string.IsNullOrEmpty(source))
                return source;
            return revision;
        }

        protected override CameraInfo DoGetItem(ZipFile zip, string fileName, string productName, ZipEntry entry)
        {
            return CameraProvider.GetCamera(productName, fileName);
        }
    }
}
