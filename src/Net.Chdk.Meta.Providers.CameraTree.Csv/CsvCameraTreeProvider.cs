using Net.Chdk.Meta.Model;
using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Providers.Csv;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraTree.Csv
{
    sealed class CsvCameraTreeProvider : CsvCameraProvider<TreePlatformData, TreeRevisionData, PlatformSourceData>, IInnerCameraTreeProvider
    {
        public IDictionary<string, TreePlatformData> GetCameraTree(string path)
        {
            return GetCameras(path);
        }

        protected override PlatformSourceData GetSourceData(string[] split)
        {
            var source = base.GetSourceData(split);
            source.Platform = split[0];
            return source;
        }
    }
}
