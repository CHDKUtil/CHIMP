using Net.Chdk.Model.Card;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Product;
using System.IO;

namespace Chimp.Services
{
    sealed class BootService : BootServiceBase, IBootService
    {
        private IProductProvider ProductProvider { get; }

        public BootService(IVolumeContainer volumeContainer, IProductProvider productProvider, IBootProvider bootProvider)
            : base(volumeContainer, bootProvider)
        {
            ProductProvider = productProvider;
        }

        public string TestBootable(CardInfo cardInfo, string fileSystem)
        {
            if (fileSystem == null)
                return null;

            var categoryNames = ProductProvider.GetCategoryNames();
            foreach (var categoryName in categoryNames)
                if (Test(cardInfo, categoryName, fileSystem))
                    return categoryName;

            return null;
        }

        public bool SetBootable(CardInfo cardInfo, string fileSystem, string categoryName, bool value)
        {
            if (fileSystem == null)
                return false;

            if (value)
            {
                var filePath = GetFileName(categoryName);
                if (!File.Exists(filePath))
                    return false; 
            }

            return Set(cardInfo, categoryName, fileSystem, value);
        }
    }
}
