using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class PsCameraModelProvider : CameraModelProvider<PsCameraModelData>, IPsCameraModelProvider
    {
        private IRevisionProvider RevisionProvider { get; }

        public PsCameraModelProvider(IRevisionProvider revisionProvider, ICameraModelValidator modelValidator)
            : base(modelValidator)
        {
            RevisionProvider = revisionProvider;
        }

        public override PsCameraModelData GetModel(string platform, string[] names, ListPlatformData list, TreePlatformData tree, string productName)
        {
            var model = base.GetModel(platform, names, list, tree, productName);
            model.Revisions = RevisionProvider.GetRevisions(productName, list, tree);
            return model;
        }
    }
}
