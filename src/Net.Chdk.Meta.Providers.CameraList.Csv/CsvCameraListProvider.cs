using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Providers.Csv;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraList.Csv
{
    sealed class CsvCameraListProvider : CsvCameraProvider<ListPlatformData, ListRevisionData, ListSourceData>, IInnerCameraListProvider
    {
        public IDictionary<string, ListPlatformData> GetCameraList(string path, string productName)
        {
            return GetCameras(path);
        }

        protected override ListRevisionData GetRevisionData(string[] split)
        {
            var revision = base.GetRevisionData(split);
            revision.Skip = "SKIP_AUTOBUILD".Equals(split[4], StringComparison.Ordinal);
            revision.Status = split[2].ToLowerInvariant();
            return revision;
        }
    }
}
