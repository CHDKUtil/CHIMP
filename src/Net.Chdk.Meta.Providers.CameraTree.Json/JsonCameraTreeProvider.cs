using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Providers.Json;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraTree.Json
{
    sealed class JsonCameraTreeProvider : JsonCameraProvider<TreePlatformData, TreeRevisionData, TreeSourceData>, IInnerCameraTreeProvider
    {
        #region IInnerCameraTreeProvider Members

        public IDictionary<string, TreePlatformData> GetCameraTree(string path)
        {
            return GetCameras(path);
        }

        #endregion
    }
}
