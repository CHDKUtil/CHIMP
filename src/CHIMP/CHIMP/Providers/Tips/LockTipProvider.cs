using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Net.Chdk.Providers.Camera;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Tips
{
    sealed class LockTipProvider : TipProvider
    {
        private ICameraProvider CameraProvider { get; }

        public LockTipProvider(MainViewModel mainViewModel, ICameraProvider cameraProvider)
            : base(mainViewModel)
        {
            CameraProvider = cameraProvider;
        }

        public override IEnumerable<Tip> GetTips(string productText)
        {
            yield return new Tip
            {
                Header = GetHeader(),
                Contents = GetContents(productText).ToArray()
            };
        }

        private string GetHeader()
        {
			return IsDoor
                ? Resources.Summary_OpenCardDoor_Header
				: Resources.Summary_LockCard_Header;
		}

		private IEnumerable<string> GetContents(string productText)
        {
            var cardFormat = IsDoor
                ? Resources.Summary_OpenCardDoor_Format
                : Resources.Summary_LockCard_Format;
            yield return string.Format(cardFormat, productText);

            if (MainViewModel.IsCompleted)
            {
                var splashString = Resources.ResourceManager.GetString($"Summary_SplashScreen_{ProductName}_Text");
                if (splashString != null)
                    yield return splashString;
            }
        }

        private bool IsDoor
        {
            get
            {
                var product = DownloadViewModel?.Software?.Product ?? SoftwareViewModel?.SelectedItem?.Info?.Product;
                var camera = CameraViewModel?.Info;
                if (product == null || camera == null)
                    return false;
                var cardType = CameraProvider.GetCardType(product, camera);
                return "microSD" == cardType;
            }
        }
    }
}
