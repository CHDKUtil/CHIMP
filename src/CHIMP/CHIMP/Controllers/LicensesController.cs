using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Chimp.Controllers
{
    sealed class LicensesController : Controller<LicensesController, LicensesViewModel>
    {
        protected override string StepName => "Licenses";
        protected override bool CanSkipStep => true;

        public LicensesController(MainViewModel viewModel, ILoggerFactory loggerFactory)
            : base(viewModel, loggerFactory)
        {
        }

        protected override void Initialize()
        {
			base.Initialize();
			ViewModel = CreateViewModel();
        }

        protected override void EnterStep()
        {
            Subscribe2();
            UpdateCanContinue();
            UpdateIsPaused();
        }

        protected override void LeaveStep()
        {
            Unsubscribe2();
        }

        private void Subscribe2()
        {
            if (ViewModel.Licenses != null)
            {
                PropertyChangedEventManager.AddHandler(ViewModel, OnIsAllAcceptedChanged, nameof(LicensesViewModel.IsAllAccepted));
                PropertyChangedEventManager.AddHandler(ViewModel, OnIsAllRejectedChanged, nameof(LicensesViewModel.IsAllRejected));
                foreach (var license in ViewModel.Licenses)
                    PropertyChangedEventManager.AddHandler(license, OnIsAcceptedChanged, nameof(LicensesItemViewModel.IsAccepted));
            }
        }

        private void Unsubscribe2()
        {
            if (ViewModel.Licenses != null)
            {
                PropertyChangedEventManager.RemoveHandler(ViewModel, OnIsAllAcceptedChanged, nameof(LicensesViewModel.IsAllAccepted));
                PropertyChangedEventManager.RemoveHandler(ViewModel, OnIsAllRejectedChanged, nameof(LicensesViewModel.IsAllRejected));
                foreach (var license in ViewModel.Licenses)
                    PropertyChangedEventManager.RemoveHandler(license, OnIsAcceptedChanged, nameof(LicensesItemViewModel.IsAccepted));
            }
        }

        private void UpdateCanContinue()
        {
            StepViewModel.CanContinue = ViewModel.IsAllAccepted;
        }

        private void UpdateIsPaused()
        {
            MainViewModel.IsWarning = ViewModel.Licenses != null && !ViewModel.IsAllAccepted;
        }

        private LicensesViewModel CreateViewModel()
        {
            var vms = GetLicenses();
            if (vms != null)
                vms[0].IsExpanded = true;
            var title = !SkipStep ? Resources.Licenses_Title_Text : null;
            return new LicensesViewModel
            {
                Title = title,
                Licenses = vms,
                IsAllRejected = true,
            };
        }

        private LicensesItemViewModel[] GetLicenses()
        {
            if (SkipStep)
                return null;
            return DoGetLicenses()
                .Select(CreateLicense)
                .ToArray();
        }

        private static Model.License[] DoGetLicenses()
        {
            var filePath = Path.Combine("Data", "licenses.json");
            using (var reader = File.OpenText(filePath))
            using (var jsonReader = new JsonTextReader(reader))
            {
                return JsonSerializer.CreateDefault().Deserialize<Model.License[]>(jsonReader);
            }
        }

        private static LicensesItemViewModel CreateLicense(Model.License license)
        {
            return new LicensesItemViewModel
            {
                Product = license.Product,
                Contents = license.Contents,
            };
        }

        private bool isAcceptingAll;

        private void OnIsAllAcceptedChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ViewModel.IsAllAccepted)
            {
                if (!isAcceptingAll)
                {
                    isAcceptingAll = true;
                    Logger.LogTrace("Accepted All");
                    foreach (var license in ViewModel.Licenses)
                        license.IsAccepted = true;
                    isAcceptingAll = false;
                }
            }

            UpdateCanContinue();
            UpdateIsPaused();
        }

        private bool isRejectingAll;

        private void OnIsAllRejectedChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ViewModel.IsAllRejected)
            {
                if (!isRejectingAll)
                {
                    isRejectingAll = true;
                    Logger.LogTrace("Rejected All");
                    foreach (var license in ViewModel.Licenses)
                        license.IsAccepted = false;
                    isRejectingAll = false;
                }
            }
        }

        private void OnIsAcceptedChanged(object sender, PropertyChangedEventArgs e)
        {
            if (isAcceptingAll || isRejectingAll)
                return;

            var license = sender as LicensesItemViewModel;
            if (license.IsAccepted)
                Logger.LogTrace("Accepted {0}", license.Product);
            else
                Logger.LogTrace("Rejected {0}", license.Product);

            ViewModel.IsAllAccepted =
                ViewModel.Licenses.All(l => l.IsAccepted);

            ViewModel.IsAllRejected =
                ViewModel.Licenses.All(l => !l.IsAccepted);
        }
    }
}
