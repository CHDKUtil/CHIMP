namespace Chimp
{
    interface IInstallerProvider
    {
        IInstaller? GetInstaller(string fileSystem);
    }
}
