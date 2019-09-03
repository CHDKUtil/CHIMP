﻿using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class CameraModelProvider<TModel>
        where TModel : CameraModelData, new()
    {
        private ICameraModelValidator ModelValidator { get; }

        protected CameraModelProvider(ICameraModelValidator modelValidator)
        {
            ModelValidator = modelValidator;
        }

        public virtual TModel GetModel(string platform, string[] names, ListPlatformData list, TreePlatformData tree, string productName)
        {
            ModelValidator.Validate(platform, list, tree, productName);
            return new TModel
            {
                Platform = platform,
                Names = names,
            };
        }
    }
}
