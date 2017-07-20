using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Providers.Tips
{
    sealed class EjectTipProvider : TipProvider
    {
        public EjectTipProvider(MainViewModel mainViewModel)
            : base(mainViewModel)
        {
        }

        public override IEnumerable<Tip> GetTips(string productText)
        {
            var isEjected = EjectViewModel?.IsCompleted;
            if (isEjected != true)
            {
                yield return new Tip
                {
                    Header = Resources.Summary_Eject_Header,
                    Contents = GetContents(isEjected).ToArray(),
                };
            }
        }

        private static IEnumerable<string> GetContents(bool? isEjected)
        {
            yield return Resources.Summary_Eject_Text;
        }

        private EjectViewModel EjectViewModel => EjectViewModel.Get(MainViewModel);
    }
}
