using Net.Chdk.Model.Software;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    public interface IFileSystemModulesDetector
    {
        ModulesInfo GetModules(SoftwareInfo software, string basePath, IProgress<double> progress, CancellationToken token);
    }
}
