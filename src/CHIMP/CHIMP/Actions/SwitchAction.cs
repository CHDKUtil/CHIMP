using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;

namespace Chimp.Actions
{
    sealed class SwitchAction : ActionBase
    {
        private int Part { get; }
        private IPartitionService PartitionService { get; }
        public override string DisplayName
        {
            get
            {
                switch (Part)
                {
                    case 1:
                        return Resources.Action_Switch_1_Text;
                    case 2:
                        return Resources.Action_Switch_2_Text;
                    case 3:
                        return Resources.Action_Switch_3_Text;
                    default:
                        return null;
                }
            }
        }

        public SwitchAction(MainViewModel mainViewModel, IPartitionService partitionService, int part)
            : base(mainViewModel)
        {
            PartitionService = partitionService;
            Part = part;
        }

        protected override SoftwareData Perform()
        {
            var driveLetter = CardViewModel.SelectedItem.Info.DriveLetter;
            if (PartitionService.SwitchPartitions(driveLetter, Part))
            {
                PartitionService.UpdateProperties(driveLetter);
                DownloadViewModel.Title = Resources.Action_Switch_Completed_Text;
            }
            return null;
        }
    }
}
