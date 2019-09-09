using Net.Chdk.Model.Card;
using Net.Chdk.Model.Software;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    public interface ISoftwareDetector
    {
        SoftwareInfo[] GetSoftware(CardInfo cardInfo, IProgress<double> progress, CancellationToken token);
    }
}
