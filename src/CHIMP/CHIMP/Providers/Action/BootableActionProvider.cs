using Chimp.Actions;
using Chimp.ViewModels;
using Net.Chdk.Model.Card;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Category;
using System.Collections.Generic;
using System.IO;

namespace Chimp.Providers.Action
{
    sealed class BootableActionProvider : ActionProvider
    {
        private ICategoryProvider CategoryProvider { get; }
        private IBootProvider BootProvider { get; }

        public BootableActionProvider(MainViewModel mainViewModel, IServiceActivator serviceActivator, ICategoryProvider categoryProvider, IBootProvider bootProvider)
            : base(mainViewModel, serviceActivator)
        {
            CategoryProvider = categoryProvider;
            BootProvider = bootProvider;
        }

        public override IEnumerable<IAction> GetActions()
        {
            var categoryName = CardViewModel?.SelectedItem?.Bootable;
            if (categoryName != null)
            {
                var types = new[] { typeof(string) };
                var values = new[] { categoryName };
                yield return ServiceActivator.Create<ClearBootableAction>(types, values);
            }
            else if ((categoryName = GetCategoryName()) != null)
            {
                var types = new[] { typeof(string) };
                var values = new[] { categoryName };
                yield return ServiceActivator.Create<SetBootableAction>(types, values);
            }
        }

        private string? GetCategoryName()
        {
            if (SoftwareViewModel?.SelectedItem == null)
                return null;

            var card = CardViewModel?.SelectedItem;
            if (card?.Switched == true || card?.Scriptable == true)
                return null;

            return DoGetCategoryName();
        }

        private string? DoGetCategoryName()
        {
            var categoryNames = CategoryProvider.GetCategoryNames();
            foreach (var categoryName in categoryNames)
            {
                var fileName = BootProvider.GetFileName(categoryName);
                var filePath = GetPath(fileName);
                if (File.Exists(filePath))
                    return categoryName;
            }
            return null;
        }

        private string GetPath(string fileName)
        {
            var rootPath = CardViewModel?.SelectedItem?.Info?.GetRootPath();
            return Path.Combine(rootPath, fileName);
        }
    }
}
