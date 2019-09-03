using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Providers.Json;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraList.Json
{
    sealed class JsonCameraListProvider : JsonCameraProvider<ListPlatformData, ListRevisionData, ListSourceData>, IInnerCameraListProvider
    {
        #region IInnerCameraListProvider Members

        public IDictionary<string, ListPlatformData> GetCameraList(string path, string productName)
        {
            return GetCameras(path);
        }

        #endregion
    }
}
