using Net.Chdk.Meta.Model.CameraTree;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraTree
{
    sealed class CameraTreeProvider : SingleExtensionProvider<IInnerCameraTreeProvider>, ICameraTreeProvider
    {
        public CameraTreeProvider(IEnumerable<IInnerCameraTreeProvider> innerProviders)
            : base(innerProviders)
        {
        }

        public IDictionary<string, TreePlatformData> GetCameraTree(string path)
        {
            var provider = GetInnerProvider(path, out string ext);
            if (provider == null)
                throw new InvalidOperationException($"Unknown camera tree extension: {ext}");
            return provider.GetCameraTree(path);
        }
    }
}
