using System;

namespace Net.Chdk.Meta.Providers.Address.Src
{
    sealed class RevisionData
    {
        public ushort? Id { get; set; }
        public uint? IdAddress { get; set; }
        public Version? Digic { get; set; }
    }
}
