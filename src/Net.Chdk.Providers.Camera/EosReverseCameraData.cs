using System;
using System.Collections.Generic;

namespace Net.Chdk.Providers.Camera
{
    sealed class EosReverseCameraData : ReverseCameraData
    {
        public Dictionary<string, Version> Versions { get; set; }
    }
}
