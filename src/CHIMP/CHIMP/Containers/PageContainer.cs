using Chimp.Providers;
using Chimp.ViewModels;
using Net.Chdk;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace Chimp.Containers
{
    sealed class PageContainer : Provider<Page>, IPageContainer, IStepProvider
    {
        private readonly ConcurrentDictionary<string, Page> pages = new ConcurrentDictionary<string, Page>();

        private MainViewModel MainViewModel { get; }

        public PageContainer(IServiceActivator serviceProvider, MainViewModel mainViewModel)
            : base(serviceProvider)
        {
            MainViewModel = mainViewModel;
        }

        public Page GetPage(string name)
        {
            return pages.GetOrAdd(name, CreatePage);
        }

        private Page CreatePage(string name)
        {
            var page = CreateProvider(Data[name], name);
            page.DataContext = MainViewModel;
            return page;
        }

        public IEnumerable<string> GetSteps()
        {
            return Data.Keys;
        }

        protected override string GetFilePath()
        {
            return Path.Combine(Directories.Data, "steps.json");
        }

        protected override string Namespace => "Chimp.Pages";

        protected override string TypeSuffix => "Page";
    }
}
