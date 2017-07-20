using Net.Chdk.Model.Software;
using System;
using System.Threading;

namespace Chimp
{
    interface IMetadataService
    {
        SoftwareInfo Update(SoftwareInfo software, string destPath, IProgress<double> progress, CancellationToken token);
    }
}
