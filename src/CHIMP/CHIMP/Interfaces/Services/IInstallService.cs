namespace Chimp
{
    interface IInstallService
    {
        void CreatePartitions();
        void SwitchPartitions();
        bool Format(string fileSystem, string label);
        bool SetBootable(string fileSystem);
        bool CopyAllFiles();
        bool CopyPrimaryFiles();
        bool CopySecondaryFiles();
        bool? TestSwitchedPartitions();
        bool ShowFormatWarning();
    }
}
