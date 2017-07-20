using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Action
{
    abstract class InstallActionProvider<TAction> : ActionProvider
        where TAction : InstallActionBase
    {
        protected ISourceProvider SourceProvider { get; }

        public InstallActionProvider(MainViewModel mainViewModel, ISourceProvider sourceProvider, IServiceActivator serviceActivator)
            : base(mainViewModel, serviceActivator)
        {
            SourceProvider = sourceProvider;
        }

        public override IEnumerable<IAction> GetActions()
        {
            return GetProducts()
                .SelectMany(GetActions);
        }

        private IEnumerable<IAction> GetActions(SoftwareProductInfo product)
        {
            return GetSources(product)
                .Select(CreateAction);
        }

        protected abstract IEnumerable<SoftwareProductInfo> GetProducts();

        protected virtual IEnumerable<ProductSource> GetSources(SoftwareProductInfo product)
        {
            if (product?.Name != null)
                return SourceProvider.GetSources(product);
            var category = GetCategory();
            return SourceProvider.GetSources(category);
        }

        private IAction CreateAction(ProductSource productSource)
        {
            var types = new[]
            {
                typeof(ProductSource)
            };
            var values = new object[]
            {
                productSource
            };
            return ServiceActivator.Create<TAction>(types, values);
        }

        private CategoryInfo GetCategory()
        {
            return new CategoryInfo
            {
                Name = GetCategoryName()
            };
        }

        private string GetCategoryName()
        {
            return CameraViewModel.Info.Canon.FirmwareRevision > 0
                ? "PS"
                : "EOS";
        }
    }
}
