using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Net.Chdk.Providers.Camera;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Tips
{
    sealed class AltTipProvider : TipProvider
    {
        private ICameraProvider CameraProvider { get; }

        public AltTipProvider(MainViewModel mainViewModel, ICameraProvider cameraProvider)
            : base(mainViewModel)
        {
            CameraProvider = cameraProvider;
        }

        public override IEnumerable<Tip> GetTips(string productText)
        {
			if (MainViewModel.IsCompleted)
            {
                yield return new Tip
                {
                    Header = Resources.Summary_AltMode_Header,
                    Contents = GetContents(productText).ToArray(),
                };
            }
        }

        private IEnumerable<string> GetContents(string productText)
        {
            var altButton = GetAltButton();
            if (altButton != null)
            {
                var keyText = Resources.ResourceManager.GetString($"Key_{altButton}") ?? altButton;
                yield return string.Format(Resources.Summary_AltMode_Format, keyText, productText);

                var altText2 = Resources.ResourceManager.GetString($"Summary_AltMode_{altButton}_Text");
                if (altText2 != null)
                    yield return altText2;
            }
        }

        private string? GetAltButton()
        {
            var software = DownloadViewModel?.Software
                ?? SoftwareViewModel?.SelectedItem?.Info;
            var alt = CameraProvider.GetAlt(software?.Product, software?.Camera);
            return alt?.Button;
        }
    }
}
