using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class CameraData
    {
        public CameraData()
        {
            Alt = new TreeAltData();
            Card = new TreeCardData();
        }

        public TreeAltData Alt { get; }
        public TreeCardData Card { get; }
    }
}
