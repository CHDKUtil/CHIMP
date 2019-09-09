using Net.Chdk.Model.Card;
using Net.Chdk.Model.Software;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    interface IInnerModulesDetector
    {
        ModulesInfo GetModules(CardInfo card, CardInfo card2, SoftwareInfo software, IProgress<double> progress, CancellationToken token);
    }
}
