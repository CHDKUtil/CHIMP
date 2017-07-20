using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using System.Collections.Generic;

namespace Chimp.Providers.Tips
{
    sealed class RemoveTipProvider : TipProvider
    {
        public RemoveTipProvider(MainViewModel mainViewModel)
            : base(mainViewModel)
        {
        }

        public override IEnumerable<Tip> GetTips(string productText)
        {
            yield return new Tip
            {
                Header = Resources.Summary_Remove_Header,
                Contents = new[]
                {
                    string.Format(Resources.Summary_EOS_Remove_Format, productText),
                    Resources.Summary_EOS_Remove_2_Text,
                    Resources.Summary_EOS_Remove_3_Text,
                }
            };
        }
    }
}
