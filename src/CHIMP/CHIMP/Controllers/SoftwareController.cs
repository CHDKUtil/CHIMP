using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Net.Chdk.Detectors.Software;
using Net.Chdk.Model.Card;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Product;
using Net.Chdk.Providers.Software;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Chimp.Controllers
{
    sealed class SoftwareController : Controller<SoftwareController, SoftwareViewModel>
    {
        protected override string StepName => "Software";
        protected override bool CanSkipStep => ViewModel?.IsCompleted == true;

        private SynchronizationContext SynchronizationContext { get; }
        private ISoftwareDetector SoftwareDetector { get; }
        private IModulesDetector ModulesDetector { get; }
        private IModuleProvider ModuleProvider { get; }
        private IProductProvider ProductProvider { get; }
        private IResourceProvider ResourceProvider { get; }

        public SoftwareController(MainViewModel mainViewModel, SynchronizationContext synchronizationContext, ISoftwareDetector softwareDetector, IModulesDetector modulesDetector,
            IModuleProvider moduleProvider, IProductProvider productProvider, IResourceProvider resourceProvider, ILoggerFactory loggerFactory)
            : base(mainViewModel, loggerFactory)
        {
            SynchronizationContext = synchronizationContext;
            SoftwareDetector = softwareDetector;
            ModulesDetector = modulesDetector;
            ModuleProvider = moduleProvider;
            ProductProvider = productProvider;
            ResourceProvider = resourceProvider;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Subscribe();
        }

        public override void Dispose()
        {
            base.Dispose();
            Unsubscribe();
        }

        protected override void EnterStep()
        {
            if (ViewModel == null)
            {
                cts = new CancellationTokenSource();
                try
                {
                    ViewModel = CreateViewModel(cts.Token);
                    ViewModel.IsCompleted = true;
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerExceptions.All(ex2 => typeof(OperationCanceledException).IsAssignableFrom(ex2.GetType())))
                        Logger.LogWarning(0, ex, "Canceled");
                    else
                        throw;
                }
                finally
                {
                    cts = null;
                }
            }
            Subscribe2();
            UpdateCanContinue();
            UpdateIsPaused();
        }

        protected override void LeaveStep()
        {
            base.LeaveStep();
            Unsubscribe2();
        }

        protected override void Card_SelectedItemChanged()
        {
            ViewModel = null;
        }

        private void Subscribe2()
        {
            if (ViewModel != null)
                ViewModel.PropertyChanged += Software_PropertyChanged;
        }

        private void Unsubscribe2()
        {
            if (ViewModel != null)
                ViewModel.PropertyChanged -= Software_PropertyChanged;
        }

        private SoftwareViewModel CreateViewModel(CancellationToken token)
        {
            var viewModel = new SoftwareViewModel();
            Update(viewModel, token);
            return viewModel;
        }

        private SoftwareItemViewModel CreateItemViewModel(SoftwareInfo software, ModulesInfo modules)
        {
            return new SoftwareItemViewModel
            {
                DisplayName = GetDisplayName(software),
                Info = software,
                Modules = CreateModulesViewModel(software, modules),
            };
        }

        private static string GetDisplayName(SoftwareInfo software)
        {
            var productName = software.Product?.Name;
            if (productName == null)
                return Resources.Software_Product_Unknown_Text;

            return Resources.ResourceManager.GetString($"Product_{productName}")
                ?? productName;
        }

        private ModulesViewModel CreateModulesViewModel(SoftwareInfo software, ModulesInfo modules)
        {
            var productName = software.Product?.Name;
            if (productName == null)
                return null;
            var items = modules.Modules?
                .Select(kvp => CreateModuleItemViewModel(software, kvp.Key, kvp.Value))
                .Where(vm => vm != null)
                .ToArray();
            return new ModulesViewModel
            {
                Items = items,
            };
        }

        private ModulesItemViewModel CreateModuleItemViewModel(SoftwareInfo software, string name, ModuleInfo module)
        {
            var displayName = GetModuleTitle(software, name);
            if (string.IsNullOrEmpty(displayName))
                return null;

            return new ModulesItemViewModel
            {
                Name = name,
                Info = module,
                DisplayName = displayName,
                ToolTip = $"{name}\n{module.Changeset}\n{module.Created}"
            };
        }

        private string GetModuleTitle(SoftwareInfo software, string name)
        {
            return null
                //?? GetModuleTitle(software, name)
                ?? TryGetModuleTitle(software, name)
                //?? EnglishGetModuleTitle(software, name)
                ?? DoGetModuleTitle(software, name)
                ?? GetDefaultModuleTitle(software, name);
        }

        //private string GetModuleTitle(SoftwareInfo software, string name)
        //{
        //    var productName = software.Product.Name;
        //    if (!productName.Equals("CHDK"))
        //        return null;
        //    name = name.Replace('.', '_');
        //    return CHDK.ResourceManager.GetString($"Module_{name}");
        //}

        private string TryGetModuleTitle(SoftwareInfo software, string name)
        {
            var productName = software.Product.Name;
            var id = ModuleProvider.GetModuleId(productName, name);
            if (id == null)
                return null;
            return ResourceProvider.GetString(id);
        }

        //private string EnglishGetModuleTitle(SoftwareInfo software, string name)
        //{
        //    var productName = software.Product.Name;
        //    var id = ModuleProvider.GetModuleId(productName, name);
        //    if (id == null)
        //        return null;
        //    return EnglishResourceProvider.GetString(id);
        //}

        private string DoGetModuleTitle(SoftwareInfo software, string name)
        {
            var productName = software.Product.Name;
            return ModuleProvider.GetModuleTitle(productName, name);
        }

        private string GetDefaultModuleTitle(SoftwareInfo software, string name)
        {
            return name;
        }

        private void Update(SoftwareViewModel viewModel, CancellationToken token)
        {
            MainViewModel.IsIndeterminate = true;

            var card = CardViewModel.SelectedItem.Info;
            var card2 = GetCard2();
            var progress = new Progress<double>(OnProgressChanged);
            var software = SoftwareDetector.GetSoftware(card, progress, token);
            foreach (var item in software)
                Logger.LogObject(LogLevel.Information, "Detected {0}", item);

            var vms = new SoftwareItemViewModel[software.Length];
            for (var i = 0; i < software.Length; i++)
            {
                var modules = ModulesDetector.GetModules(card, card2, software[i], progress, token);
                Logger.LogObject(LogLevel.Information, "Detected {0}", modules);
                vms[i] = CreateItemViewModel(software[i], modules);
            }
            viewModel.Items = vms;

            viewModel.SelectedItem = viewModel.Items.Length == 1
                ? viewModel.Items.Single()
                : null;
            viewModel.IsSelect = viewModel.Items.Length > 1;

            var title = GetTitle(software);
            Logger.LogInformation(title);
            viewModel.Title = title;
        }

        private CardInfo GetCard2()
        {
            var card = CardViewModel.SelectedItem;
            if (card.Switched != false)
            {
                return card.Info;
            }

            //TODO Use the other volume on Windows 10.2
            return null;
        }

        private string GetTitle(SoftwareInfo[] software)
        {
            if (software.Length == 0)
                return nameof(Resources.Software_Title_None_Text);
            if (software.Length > 1)
                return nameof(Resources.Software_Title_Select_Text);
            if (IsKnown(software[0]))
                return nameof(Resources.Software_Title_Unknown_Text);
            if (!IsSupported(software[0]))
                return nameof(Resources.Software_Title_Unsupported_Text);
            return nameof(Resources.Software_Title_Detected_Text);
        }

        private void OnProgressChanged(double value)
        {
            UpdateProgress(value);
            //SynchronizationContext.Post(_ => UpdateProgress(value), null);
        }

        private void UpdateProgress(double value)
        {
            MainViewModel.IsIndeterminate = false;
            MainViewModel.ProgressValue = value;
        }

        private void UpdateCanContinue()
        {
            StepViewModel.CanContinue = ViewModel != null && (ViewModel.Items.Length == 0 || ViewModel.SelectedItem != null);
        }

        private void UpdateIsPaused()
        {
            MainViewModel.IsWarning = ViewModel != null && ViewModel.Items.Length > 0 && ViewModel.SelectedItem == null;
        }

        private void Software_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(SoftwareViewModel.SelectedItem):
                    Logger.LogObject(LogLevel.Information, "Selected {0}", ViewModel.SelectedItem?.Info);
                    UpdateCanContinue();
                    UpdateIsPaused();
                    break;
                default:
                    break;
            }
        }

        private static bool IsKnown(SoftwareInfo software)
        {
            return software.Product?.Name == null;
        }

        private bool IsSupported(SoftwareInfo software)
        {
            return ProductProvider.GetProductNames()
                .Contains(software.Product.Name, StringComparer.InvariantCulture);
        }
    }
}
