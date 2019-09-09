using Net.Chdk.Detectors.Software.Product;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;

namespace Net.Chdk.Detectors.Software.Sdm
{
    abstract class SdmSoftwareDetectorBase : ProductBinarySoftwareDetector
    {
        protected SdmSoftwareDetectorBase(IProductProvider productProvider, ISourceProvider sourceProvider)
            : base(productProvider, sourceProvider)
        {
        }

        public sealed override string ProductName => "SDM";

        protected override SoftwareBuildInfo GetBuild(string[] strings)
        {
            return new SoftwareBuildInfo();
        }
    }
}
