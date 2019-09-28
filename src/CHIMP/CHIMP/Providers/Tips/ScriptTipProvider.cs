using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using System.Collections.Generic;

namespace Chimp.Providers.Tips
{
    sealed class ScriptTipProvider : TipProvider
    {
        public ScriptTipProvider(MainViewModel mainViewModel)
            : base(mainViewModel)
        {
        }

        public override IEnumerable<Tip> GetTips(string productText)
        {
            if (MainViewModel.IsCompleted)
            {
                yield return new Tip
                {
                    Header = Resources.Summary_Script_Header,
                    Contents = new[]
                    {
                        Resources.Summary_Script_Text,
                        string.Format(Resources.Summary_Script_Format, productText)
                    }
                };
            }
        }
    }
}
