using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Net.Chdk.Generators.Script;
using Net.Chdk.Model.Card;
using System.Collections.Generic;

namespace Chimp.Actions
{
    sealed class ClearOverlaysAction : ActionBase
    {
        private IScriptGenerator ScriptGenerator { get; }
        private IScriptService ScriptService { get; }
        private IDictionary<string, string> Substitutes { get; }

        private CardInfo Card => CardViewModel.SelectedItem.Info;

        public ClearOverlaysAction(MainViewModel mainViewModel, IScriptGenerator scriptGenerator, IScriptService scriptService, IDictionary<string, string> substitutes)
            : base(mainViewModel)
        {
            ScriptService = scriptService;
            ScriptGenerator = scriptGenerator;
            Substitutes = substitutes;
        }

        public override string DisplayName => Resources.Action_ClearOverlays_Text;

        protected override SoftwareData Perform()
        {
            ScriptGenerator.GenerateScript(Card, "clear_overlays", Substitutes);
            if (ScriptService.SetScriptable(Card, Card.FileSystem, true))
            {
                CardViewModel.SelectedItem.Scriptable = ScriptService.TestScriptable(Card, Card.FileSystem);
                DownloadViewModel.Title = Resources.Action_ClearOverlays_Completed_Text;
            }
            return null;
        }
    }
}
