using Chimp.Model;

namespace Chimp
{
    public interface IPartitionService
    {
        void CreateSinglePartition(string driveLetter);
        void CreatePartitions(string driveLetter);
        bool SwitchPartitions(string driveLetter, int part);
        void UpdateProperties(string driveLetter);
        PartitionType[] GetPartitionTypes(string driveLetter);
        bool? TestSwitchedPartitions(PartitionType[] partTypes);
    }
}
