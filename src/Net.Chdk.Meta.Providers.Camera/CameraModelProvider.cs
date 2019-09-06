using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    sealed class CameraModelProvider<TModel> : ICameraModelProvider<TModel>
        where TModel : CameraModelData, ICameraModelData, new()
    {
        private ICameraModelValidator ModelValidator { get; }
        private IRevisionProvider RevisionProvider { get; }

        public CameraModelProvider(ICameraModelValidator modelValidator, IRevisionProvider revisionProvider)
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
