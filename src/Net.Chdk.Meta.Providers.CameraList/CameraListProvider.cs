using Net.Chdk.Meta.Model.CameraList;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraList
{
    sealed class CameraListProvider : SingleExtensionProvider<IInnerCameraListProvider>, ICameraListProvider
    {
        public CameraListProvider(IEnumerable<IInnerCameraListProvider> innerProviders)
            : base(innerProviders)
        {
        }

        public IDictionary<string, ListPlatformData> GetCameraList(string path, string productName)
        {
            var provider = GetInnerProvider(path, out string ext);
            if (provider == null)
                throw new InvalidOperationException($"Unknown camera list extension: {ext}");
            return provider.GetCameraList(path, productName);
        }
    }
}
