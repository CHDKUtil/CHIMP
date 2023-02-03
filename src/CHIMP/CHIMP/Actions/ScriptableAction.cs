using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Net.Chdk.Model.Card;

namespace Chimp.Actions
{
    abstract class ScriptableAction : ActionBase
    {
        private IScriptService ScriptService { get; }
        private CardInfo Card => CardViewModel.SelectedItem.Info;
        private bool Value { get; }
        protected abstract string CompletedTitle { get; }

        protected ScriptableAction(MainViewModel mainViewModel, IScriptService scriptService, bool value)
            : base(mainViewModel)
        {
            ScriptService = scriptService;
            Value = value;
        }

        protected override SoftwareData Perform()
        {
            if (ScriptService.SetScriptable(Card, Card.FileSystem, Value) == true)
            {
                DownloadViewModel.ProgressMaximum = 1;
                DownloadViewModel.ProgressValue = 1;
                CardViewModel.SelectedItem.Scriptable = ScriptService.TestScriptable(Card, Card.FileSystem);
                //MainViewModel.Set<ActionViewModel>("Action", null);
                //MainViewModel.Step.CanGoBack = true;
                DownloadViewModel.Title = CompletedTitle;
            }
            return null;
        }
    }

    sealed class SetScriptableAction : ScriptableAction
    {
        public SetScriptableAction(MainViewModel mainViewModel, IScriptService scriptService)
            : base(mainViewModel, scriptService, true)
        {
        }

        public override string DisplayName => Resources.Action_Scriptable_Set_Text;

        protected override string CompletedTitle => Resources.Action_Scriptable_Set_Completed_Text;
    }

    sealed class ClearScriptableAction : ScriptableAction
    {
        public ClearScriptableAction(MainViewModel mainViewModel, IScriptService scriptService)
            : base(mainViewModel, scriptService, false)
        {
        }

        public override string DisplayName => Resources.Action_Scriptable_Clear_Text;

        protected override string CompletedTitle => Resources.Action_Scriptable_Clear_Completed_Text;
    }
}
