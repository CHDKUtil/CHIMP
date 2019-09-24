using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;

namespace Chimp.Actions
{
    sealed class EjectAction : ActionBase
    {
        public EjectAction(MainViewModel mainViewModel)
            : base(mainViewModel)
        {
        }

        public override string DisplayName => Resources.Action_Eject_Text;

        protected override SoftwareData? Perform()
        {
            SetTitle(DisplayName);
            return null;
        }
    }
}
