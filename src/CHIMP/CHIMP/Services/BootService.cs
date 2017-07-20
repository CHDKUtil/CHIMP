using Net.Chdk.Model.Card;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Category;
using System.IO;

namespace Chimp.Services
{
    sealed class BootService : BootServiceBase, IBootService
    {
        private ICategoryProvider CategoryProvider { get; }
        private IBootProvider BootProvider { get; }

        public BootService(IVolumeContainer volumeContainer, ICategoryProvider categoryProvider, IBootProvider bootProvider)
            : base(volumeContainer)
        {
            CategoryProvider = categoryProvider;
            BootProvider = bootProvider;
        }

        public string TestBootable(CardInfo cardInfo, string fileSystem)
        {
            if (fileSystem == null)
                return null;

            var categoryNames = CategoryProvider.GetCategoryNames();
            foreach (var categoryName in categoryNames)
            {
                var blockSize = BootProvider.GetBlockSize(categoryName, fileSystem);
                var bytes = BootProvider.GetBytes(categoryName, fileSystem);
                if (Test(cardInfo, blockSize, bytes))
                    return categoryName;
            }
            return null;
        }

        public bool SetBootable(CardInfo cardInfo, string fileSystem, string categoryName, bool value)
        {
            if (fileSystem == null)
                return false;

            if (value)
            {
                var filePath = GetPath(cardInfo, BootProvider.GetFileName(categoryName));
                if (!File.Exists(filePath))
                    return false;
            }

            var blockSize = BootProvider.GetBlockSize(categoryName, fileSystem);
            var bytes = BootProvider.GetBytes(categoryName, fileSystem);
            return Set(cardInfo, blockSize, bytes, value);
        }
    }
}
