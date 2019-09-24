using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Net.Chdk.Model.Card;

namespace Chimp.Actions
{
    abstract class BootableAction : ActionBase
    {
        private IBootService BootService { get; }
        private CardInfo? Card => CardViewModel?.SelectedItem?.Info;
        private bool Value { get; }
        protected abstract string CompletedTitle { get; }
        private string CategoryName { get; }

        protected BootableAction(MainViewModel mainViewModel, IBootService bootService, string categoryName, bool value)
            : base(mainViewModel)
        {
            BootService = bootService;
            CategoryName = categoryName;
            Value = value;
        }

        protected override SoftwareData? Perform()
        {
            var fileSystem = Card?.FileSystem;
            if (fileSystem != null && BootService.SetBootable(Card!, fileSystem, CategoryName, Value))
            {
                var card = CardViewModel?.SelectedItem;
                if (card != null)
                    card.Bootable = BootService.TestBootable(Card!, fileSystem);
                //MainViewModel.Set<ActionViewModel>("Action", null);
                //MainViewModel.Step.CanGoBack = true;
                SetTitle(CompletedTitle);
            }
            return null;
        }
    }

    sealed class SetBootableAction : BootableAction
    {
        public SetBootableAction(MainViewModel mainViewModel, IBootService bootService, string categoryName)
            : base(mainViewModel, bootService, categoryName, true)
        {
        }

        public override string DisplayName => Resources.Action_Bootable_Set_Text;

        protected override string CompletedTitle => Resources.Action_Bootable_Set_Completed_Text;
    }

    sealed class ClearBootableAction : BootableAction
    {
        public ClearBootableAction(MainViewModel mainViewModel, IBootService bootService, string categoryName)
            : base(mainViewModel, bootService, categoryName, false)
        {
        }

        public override string DisplayName => Resources.Action_Bootable_Clear_Text;

        protected override string CompletedTitle => Resources.Action_Bootable_Clear_Completed_Text;
    }
}
