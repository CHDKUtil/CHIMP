using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class CameraData
    {
        public CameraData()
        {
            Alt = new TreeAltData();
        }

        public TreeAltData Alt { get; }
        public bool MultiCard { get; set; }
    }
}
