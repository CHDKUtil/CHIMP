using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Chimp.Controllers
{
    sealed class SummaryController : Controller<SummaryController, SummaryViewModel>
    {
        protected override bool CanSkipStep => MainViewModel.IsCompleted || MainViewModel.IsAborted;

        private IEnumerable<ITipProvider> TipProviders { get; }

        public SummaryController(IEnumerable<ITipProvider> tipProviders, MainViewModel mainViewModel, IStepProvider stepProvider, string stepName, ILoggerFactory loggerFactory)
            : base(mainViewModel, stepProvider, stepName, loggerFactory)
        {
            TipProviders = tipProviders;
        }

        protected override void EnterStep()
        {
            StepViewModel.CanGoBack = false;
            if (ViewModel == null)
                ViewModel = CreateViewModel();
        }

        protected override void LeaveStep()
        {
            var basePath = DownloadViewModel?.Paths?[0];
            if (ViewModel?.IsOpenUserManual == true)
            {
                Process.Start(ViewModel.UserManualUrl);
            }
            if (basePath != null && ViewModel?.IsShowCameraNotes == true)
            {
                var path = Path.Combine(basePath, ViewModel.CameraNotesFileName);
                Process.Start(path);
            }
        }

        private SummaryViewModel CreateViewModel()
        {
            var productText = GetProductText();
            var userManualUrl = GetUserManualUrl();
            var cameraNotesFileName = GetCameraNotesFileName();
            return new SummaryViewModel
            {
                Title = GetTitle(),
                Message = GetMessage(productText),
                Tips = GetTips(productText),
                UserManualUrl = userManualUrl,
                CameraNotesFileName = cameraNotesFileName,
                IsOpenUserManual = userManualUrl != null,
                IsShowCameraNotes = cameraNotesFileName != null,
            };
        }

        private string GetProductText()
        {
            return GetProductResource("Product_{0}");
        }

        private string GetUserManualUrl()
        {
            return MainViewModel.IsCompleted
                ? GetProductResource("Summary_{0}_UserManual_Url")
                : null;
        }

        private string GetCameraNotesFileName()
        {
            return MainViewModel.IsCompleted
                ? GetProductResource("Summary_{0}_CameraNotes_FileName")
                : null;
        }

        private string GetProductResource(string keyFormat)
        {
            var software = DownloadViewModel?.Software
                ?? SoftwareViewModel?.SelectedItem?.Info;
            var productName = software?.Product?.Name;
            var format = string.Format(keyFormat, productName);
            return Resources.ResourceManager.GetString(format);
        }

        private string GetTitle()
        {
            if (SkipStep)
                return null;
            if (MainViewModel.IsCompleted)
                return nameof(Resources.Summary_Completed_Title_Text);
            if (MainViewModel.IsAborted)
                return nameof(Resources.Summary_Aborted_Title_Text);
            return null;
        }

        private string GetMessage(string productText)
        {
            if (!MainViewModel.IsCompleted || SkipStep)
                return null;

            var software = DownloadViewModel.Software;
            var versionText = software.Product.GetVersionText();
            var model = CameraViewModel.IsSelect
                ? CameraViewModel.SelectedItem.DisplayName
                : CameraViewModel.Info?.Base?.Model;
            var cardName = CardViewModel.SelectedItem.DisplayName;

            return string.Format(Resources.Summary_Completed_Format, productText, versionText, model, cardName);
        }

        private Tip[] GetTips(string productText)
        {
            if (SkipStep)
                return null;

            return TipProviders
                .SelectMany(p => GetTips(p, productText))
                .ToArray();
        }

        private IEnumerable<Tip> GetTips(ITipProvider tipsProvider, string productText)
        {
            return tipsProvider.GetTips(productText);
        }
    }
}
