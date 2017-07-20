using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Tips
{
    sealed class LockTipProvider : TipProvider
    {
        public LockTipProvider(MainViewModel mainViewModel)
            : base(mainViewModel)
        {
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
                return "microSD".Equals(CameraViewModel?.CardType, StringComparison.Ordinal);
            }
        }
    }
}
