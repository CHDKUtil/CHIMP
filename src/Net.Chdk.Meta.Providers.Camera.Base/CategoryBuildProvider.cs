using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Model.Platform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class CategoryBuildProvider<TCamera, TModel, TRevision, TCard> : ICategoryBuildProvider
        where TCamera : CameraData<TCamera, TModel, TRevision, TCard>
        where TModel : CameraModelData<TModel, TRevision>
        where TRevision : IRevisionData
        where TCard : CardData
    {
        private ICameraProvider<TCamera, TModel, TRevision, TCard> CameraProvider { get; }
        private ICameraModelProvider<TModel, TRevision> ModelProvider { get; }
        private ICameraPlatformProvider PlatformProvider { get; }
        private ICameraValidator CameraValidator { get; }

        public abstract string CategoryName { get; }

        protected CategoryBuildProvider(ICameraProvider<TCamera, TModel, TRevision, TCard> cameraProvider, ICameraModelProvider<TModel, TRevision> modelProvider,
            ICameraPlatformProvider platformProvider, ICameraValidator cameraValidator)
        {
            CameraProvider = cameraProvider;
            ModelProvider = modelProvider;
            PlatformProvider = platformProvider;
            CameraValidator = cameraValidator;
        }

        public IDictionary<string, ICameraData> GetCameras(IDictionary<string, PlatformData> platforms, IDictionary<string, ListPlatformData> list, IDictionary<string, TreePlatformData> tree,
            string productName)
        {
            CameraValidator.Validate(list, tree, productName);
            var cameras = new SortedDictionary<uint, ICameraData>();
            foreach (var kvp in list)
                AddModel(cameras, platforms, tree, kvp.Key, kvp.Value, productName);
            return cameras.ToDictionary(kvp => $"0x{kvp.Key:x}", kvp => kvp.Value);
        }

        private void AddModel(IDictionary<uint, ICameraData> cameras, IDictionary<string, PlatformData> platforms, IDictionary<string, TreePlatformData> treeCameras,
            string key, ListPlatformData list, string productName)
        {
            var platform = PlatformProvider.GetPlatform(key, platforms, productName);
            var tree = PlatformProvider.GetTree(key, treeCameras, productName);
            var modelId = Convert.ToUInt32(platform.ModelId, 16);
            var camera = GetOrAddCamera(modelId, key, list, tree, cameras, productName);
            var model = ModelProvider.GetModel(key, platform.Names, list, tree, productName);
            camera.Models = camera.Models.Concat(new[] { model }).ToArray();
        }

        private ICameraData GetOrAddCamera(uint modelId, string platform, ListPlatformData list, TreePlatformData tree, IDictionary<uint, ICameraData> cameras, string productName)
        {
            if (!cameras.TryGetValue(modelId, out ICameraData camera))
            {
                camera = CameraProvider.GetCamera(modelId, platform, list, tree, productName);
                cameras.Add(modelId, camera);
            }
            return camera;
        }
    }
}
