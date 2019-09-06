using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Software
{
    sealed class EncodingMetaProvider : IEncodingMetaProvider
    {
        #region Fields

        private int[][] Offsets { get; }

        #endregion

        #region Constructor

        public EncodingMetaProvider(IBootProvider bootProvider)
        {
            Offsets = bootProvider.GetOffsets("PS");

            _encodings = new Lazy<Dictionary<uint, SoftwareEncodingInfo>>(GetEncodings);
        }

        #endregion

        #region IEncodingMetaProvider Members

        public SoftwareEncodingInfo GetEncoding(SoftwareInfo software)
        {
            return Encodings[software.Encoding.Data ?? 0];
        }

        #endregion

        #region Encodings

        private readonly Lazy<Dictionary<uint, SoftwareEncodingInfo>> _encodings;

        private Dictionary<uint, SoftwareEncodingInfo> Encodings => _encodings.Value;

        private Dictionary<uint, SoftwareEncodingInfo> GetEncodings()
        {
            var length = Offsets != null
                ? Offsets.Length
                : 1;
            return Enumerable.Range(0, length)
                .Select(GetOffsets)
                .ToDictionary(e => e.Data ?? 0, e => e);
        }

        private SoftwareEncodingInfo GetOffsets(int i)
        {
            return i > 0
                ? GetEncoding(Offsets[i - 1])
                : SoftwareEncodingInfo.Empty;
        }

        private static SoftwareEncodingInfo GetEncoding(int[] offsets)
        {
            return new SoftwareEncodingInfo
            {
                Name = "dancingbits",
                Data = GetOffsets(offsets),
            };
        }

        private static uint? GetOffsets(int[] offsets)
        {
            var uOffsets = 0u;
            for (int index = 0; index < offsets.Length; index++)
                uOffsets += (uint)offsets[index] << (index << 2);
            return uOffsets;
        }

        #endregion
    }
}
