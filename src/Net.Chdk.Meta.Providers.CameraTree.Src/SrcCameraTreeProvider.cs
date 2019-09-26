using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Providers.Src;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class SrcCameraTreeProvider : SrcProvider<TreePlatformData, TreeRevisionData, CameraData, RevisionData, byte>, IInnerCameraTreeProvider
    {
        public SrcCameraTreeProvider(PlatformProvider platformProvider)
            : base(platformProvider)
        {
        }

        public IDictionary<string, TreePlatformData> GetCameraTree(string path)
        {
            return GetTree(path)!;
        }
    }
}
