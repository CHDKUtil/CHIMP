using Chimp.Model;
using Chimp.Providers;
using Chimp.ViewModels;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Chimp.Containers
{
    sealed class PageContainer : Provider<StepData, Page>, IPageContainer
    {
        private readonly ConcurrentDictionary<string, Page> pages = new ConcurrentDictionary<string, Page>();

        private MainViewModel MainViewModel { get; }
        private StepsData StepsData { get; }

        public PageContainer(IServiceActivator serviceActivator, MainViewModel mainViewModel, IOptions<StepsData> options)
            : base(serviceActivator)
        {
            MainViewModel = mainViewModel;
            StepsData = options.Value;
        }

        public Page GetPage(string name)
        {
            return pages.GetOrAdd(name, CreatePage);
        }

        private Page CreatePage(string name)
        {
            var page = CreateProvider(name, name);
            page.DataContext = MainViewModel;
            return page;
        }

        protected override IDictionary<string, StepData>? Data =>
            StepsData.Steps?.ToDictionary(
                s => s.Name!,
                s => s);

        protected override string GetNamespace(string key)
        {
            return Data?[key].Namespace
                ?? "Chimp.Pages";
        }

        protected override string TypeSuffix => "Page";
    }
}
