using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using System.Collections.Generic;

namespace Chimp.Providers.Tips
{
    sealed class LedTipProvider : TipProvider
    {
        public LedTipProvider(MainViewModel mainViewModel)
            : base(mainViewModel)
        {
        }

        public override IEnumerable<Tip> GetTips(string productText)
        {
            yield return new Tip
            {
                Header = Resources.Summary_EOS_Led_Header,
                Contents = new[]
                {
                    Resources.Summary_EOS_Led_Text
                }
            };
        }
    }
}
