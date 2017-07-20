using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Tips
{
    sealed class MenuTipProvider : TipProvider
    {
        public MenuTipProvider(MainViewModel mainViewModel)
            : base(mainViewModel)
        {
        }

        public override IEnumerable<Tip> GetTips(string productText)
        {
			if (MainViewModel.IsCompleted)
			{
				yield return new Tip
				{
					Header = GetHeader(productText),
					Contents = GetContents(productText).ToArray()
				};
			}
        }

        private string GetHeader(string productText)
        {
            return string.Format(Resources.Summary_Menu_Header_Format, productText);
        }

        private IEnumerable<string> GetContents(string productText)
        {
            if (ProductName != null)
            {
                var text = Resources.ResourceManager.GetString($"Summary_Menu_{ProductName}_Text");
                if (text != null)
                {
                    yield return text;
                }
                else
                {
                    var format = Resources.ResourceManager.GetString($"Summary_Menu_Format");
                    yield return string.Format(format, productText);
                }
            }
        }
    }
}
