using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    sealed class CameraModelProvider<TModel, TRevision> : ICameraModelProvider<TModel, TRevision>
        where TModel : CameraModelData<TModel, TRevision>, ICameraModelData<TModel, TRevision>, new()
        where TRevision : IRevisionData
    {
        private ICameraModelValidator ModelValidator { get; }
        private IRevisionProvider<TRevision> RevisionProvider { get; }

        public CameraModelProvider(ICameraModelValidator modelValidator, IRevisionProvider<TRevision> revisionProvider)
        {
            ModelValidator = modelValidator;
            RevisionProvider = revisionProvider;
        }

        public TModel GetModel(string platform, string[] names, ListPlatformData list, TreePlatformData tree, string productName)
        {
            ModelValidator.Validate(platform, list, tree, productName);
            return new TModel
            {
                Platform = platform,
                Names = names,
                Revisions = RevisionProvider.GetRevisions(productName, list, tree),
            };
        }
    }
}
