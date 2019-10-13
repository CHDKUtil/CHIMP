using Chimp.ViewModels;
using Net.Chdk.Model.Camera;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Action.Install
{
    abstract class ActionCreator : ActionCreatorBase
    {
        private IFirmwareProvider FirmwareProvider { get; }
        private ISourceProvider SourceProvider { get; }

        protected ActionCreator(SoftwareProductInfo product, SoftwareSourceInfo source, SoftwareCameraInfo camera, SoftwareModelInfo model, IFirmwareProvider firmwareProvider, IProductProvider productProvider, ISourceProvider sourceProvider, IServiceActivator serviceActivator)
            : base(product, source, camera, model, productProvider, serviceActivator)
        {
            FirmwareProvider = firmwareProvider;
            SourceProvider = sourceProvider;
        }

        public override IEnumerable<SoftwareProductInfo> GetProducts(CardItemViewModel card, CameraInfo camera)
        {
            var categoryName = FirmwareProvider.GetCategoryName(camera);
            if (categoryName != CategoryName)
                return Enumerable.Empty<SoftwareProductInfo>();
            return GetProducts();
        }

        public override IEnumerable<ProductSource> GetSources(SoftwareProductInfo product)
        {
            return product?.Name != null
                ? SourceProvider.GetSources(product)
                : SourceProvider.GetSources(Category);
        }
    }

    sealed class EosInstallActionCreator : ActionCreator
    {
        public EosInstallActionCreator(SoftwareProductInfo product, SoftwareSourceInfo source, SoftwareCameraInfo camera, SoftwareModelInfo model, IFirmwareProvider firmwareProvider, IProductProvider productProvider, ISourceProvider sourceProvider, IServiceActivator serviceActivator)
            : base(product, source, camera, model, firmwareProvider, productProvider, sourceProvider, serviceActivator)
        {
        }

        protected override string CategoryName => "EOS";
    }

    sealed class PsInstallActionCreator : ActionCreator
    {
        public PsInstallActionCreator(SoftwareProductInfo product, SoftwareSourceInfo source, SoftwareCameraInfo camera, SoftwareModelInfo model, IFirmwareProvider firmwareProvider, IProductProvider productProvider, ISourceProvider sourceProvider, IServiceActivator serviceActivator)
            : base(product, source, camera, model, firmwareProvider, productProvider, sourceProvider, serviceActivator)
        {
        }

        protected override string CategoryName => "PS";
    }
}
