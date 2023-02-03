using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;
using Net.Chdk.Model.Card;

namespace Chimp.Actions
{
    abstract class BootableAction : ActionBase
    {
        private IBootService BootService { get; }
        private CardInfo Card => CardViewModel.SelectedItem.Info;
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

        protected override SoftwareData Perform()
        {
            if (BootService.SetBootable(Card, Card.FileSystem, CategoryName, Value) == true)
            {
                DownloadViewModel.ProgressMaximum = 1;
                DownloadViewModel.ProgressValue = 1;
                //CardViewModel.SelectedItem.Bootable = BootService.TestBootable(Card, Card.FileSystem);
                //MainViewModel.Set<ActionViewModel>("Action", null);
                //MainViewModel.Step.CanGoBack = true;
                DownloadViewModel.Title = CompletedTitle;
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
