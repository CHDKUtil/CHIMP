using Net.Chdk.Model.Software;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    public interface IInnerBinarySoftwareDetector
    {
        SoftwareInfo GetSoftware(string basePath, string categoryName, IProgress<double> progress, CancellationToken token);
        SoftwareInfo GetSoftware(byte[] buffer, IProgress<double> progress, CancellationToken token);
        bool UpdateSoftware(SoftwareInfo software, byte[] buffer);
    }
}
