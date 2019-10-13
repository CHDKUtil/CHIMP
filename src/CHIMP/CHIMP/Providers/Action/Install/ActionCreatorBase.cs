using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Action.Install
{
    abstract class ActionCreatorBase : IActionCreator
    {
        private SoftwareProductInfo Product { get; }
        private SoftwareSourceInfo Source { get; }
        private SoftwareCameraInfo Camera { get; }
        private SoftwareModelInfo Model { get; }

        private IProductProvider ProductProvider { get; }
        private IServiceActivator ServiceActivator { get; }

        public ActionCreatorBase(SoftwareProductInfo product, SoftwareSourceInfo source, SoftwareCameraInfo camera, SoftwareModelInfo model, IProductProvider productProvider, IServiceActivator serviceActivator)
        {
            Product = product;
            Source = source;
            Camera = camera;
            Model = model;

            ProductProvider = productProvider;
            ServiceActivator = serviceActivator;

            category = new Lazy<CategoryInfo>(GetCategory);
        }

        public abstract IEnumerable<SoftwareProductInfo> GetProducts(CardItemViewModel card, CameraInfo camera);

        public abstract IEnumerable<ProductSource> GetSources(SoftwareProductInfo product);

        public TAction CreateAction<TAction>(ProductSource productSource)
            where TAction : InstallActionBase
        {
            var software = new SoftwareInfo
            {
                Category = Category,
                Product = Product,
                Source = Source,
                Camera = Camera,
                Model = Model,
            };
            var types = new[]
            {
                typeof(SoftwareInfo),
                typeof(ProductSource)
            };
            var values = new object[]
            {
                software,
                productSource
            };
            return ServiceActivator.Create<TAction>(types, values);
        }

        protected IEnumerable<SoftwareProductInfo> GetProducts()
        {
            return ProductProvider.GetProductNames()
                .Where(IsValidProduct)
                .Select(CreateProduct);
        }

        private bool IsValidProduct(string productName)
        {
            return ProductProvider.GetCategoryName(productName) == CategoryName;
        }

        private static SoftwareProductInfo CreateProduct(string productName)
        {
            return new SoftwareProductInfo
            {
                Name = productName,
            };
        }

        #region Category

        private readonly Lazy<CategoryInfo> category;
        protected CategoryInfo Category => category.Value;
        private CategoryInfo GetCategory()
        {
            return new CategoryInfo
            {
                Name = CategoryName
            };
        }

        #endregion

        protected abstract string CategoryName { get; }
    }
}
