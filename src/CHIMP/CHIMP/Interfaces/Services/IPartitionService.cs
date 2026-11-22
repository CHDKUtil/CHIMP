using Chimp.Model;

namespace Chimp
{
    public interface IPartitionService
    {
        void CreateSinglePartition(string driveLetter);
        void CreatePartitions(string driveLetter);
        bool SwitchPartitions(string driveLetter, int part);
        bool SetPartitionTypes(string driveLetter, PartitionType[] partTypes);
        void UpdateProperties(string driveLetter);
        string WaitForPartitionVolume(string driveLetter, int part);
        PartitionType[] GetPartitionTypes(string driveLetter);
        bool? TestSwitchedPartitions(PartitionType[] partTypes);
    }
}
