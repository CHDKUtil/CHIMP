using System.Collections.Generic;

namespace Net.Chdk.Providers.Camera
{
    sealed class PsReverseCameraData : ReverseCameraData
    {
        public Dictionary<string, uint> Revisions { get; set; }
    }
}
