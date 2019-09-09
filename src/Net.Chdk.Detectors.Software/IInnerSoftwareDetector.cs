using Net.Chdk.Model.Card;
using Net.Chdk.Model.Category;
using Net.Chdk.Model.Software;
using System;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    interface IInnerSoftwareDetector
    {
        SoftwareInfo GetSoftware(CardInfo cardInfo, CategoryInfo category, IProgress<double> progress, CancellationToken token);
    }
}
