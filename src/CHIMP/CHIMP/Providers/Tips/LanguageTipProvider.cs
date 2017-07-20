using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Net.Chdk;
using System.Collections.Generic;
using System.Globalization;

namespace Chimp.Providers.Tips
{
    sealed class LanguageTipProvider : TipProvider
    {
        public LanguageTipProvider(MainViewModel mainViewModel)
            : base(mainViewModel)
        {
        }

        public override IEnumerable<Tip> GetTips(string productText)
        {
            if (IsVisible)
            {
				yield return new Tip
                {
                    Header = Resources.Summary_SwitchLanguage_Header,
                    Contents = new[]
                    {
                        string.Format(Resources.Summary_SwitchLanguage_Format, productText, Language.DisplayName),
                        Resources.Summary_SwitchLanguage_2_Text,
                    }
                };
            }
        }

        private bool IsVisible
        {
            get
            {
				if (!MainViewModel.IsCompleted)
					return false;

                if (Language == null)
                    return false;

                return !Language.IsCurrentUICulture();
            }
        }

        private CultureInfo Language => DownloadViewModel.Software.Product.Language;
    }
}
