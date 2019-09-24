using Chimp.Model;
using Chimp.Properties;
using Chimp.ViewModels;

namespace Chimp.Actions
{
    sealed class SwitchAction : ActionBase
    {
        private int Part { get; }
        private IPartitionService PartitionService { get; }
        public override string? DisplayName => Part switch
        {
            1 => Resources.Action_Switch_1_Text,
            2 => Resources.Action_Switch_2_Text,
            3 => Resources.Action_Switch_3_Text,
            _ => null,
        };

        public SwitchAction(MainViewModel mainViewModel, IPartitionService partitionService, int part)
            : base(mainViewModel)
        {
            PartitionService = partitionService;
            Part = part;
        }

        protected override SoftwareData? Perform()
        {
            var driveLetter = CardViewModel?.SelectedItem?.Info?.DriveLetter;
            if (driveLetter != null && PartitionService.SwitchPartitions(driveLetter, Part))
            {
                PartitionService.UpdateProperties(driveLetter);
                SetTitle(Resources.Action_Switch_Completed_Text);
            }
            return null;
        }
    }
}
