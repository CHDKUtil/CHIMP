namespace Chimp
{
    interface IInstallService
    {
        bool CreatePartitions();
        bool SwitchPartitions();
        bool UpdateDriveLetter();
        bool RestorePartitionTypes();
        bool Format(string fileSystem, string label);
        bool SetBootable(string fileSystem);
        bool CopyAllFiles();
        bool CopyPrimaryFiles();
        bool CopySecondaryFiles();
        bool? TestSwitchedPartitions();
        bool ShowFormatWarning();
    }
}
