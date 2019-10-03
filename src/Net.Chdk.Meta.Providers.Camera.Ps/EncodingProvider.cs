using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Providers.Boot;
using System;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class EncodingProvider : IEncodingProvider
    {
        #region Fields

        private IBootProvider BootProvider { get; }

        #endregion

        #region Constructor

        public EncodingProvider(IBootProvider bootProvider)
        {
            BootProvider = bootProvider;
            _encodings = new Lazy<EncodingData[]>(GetEncodings);
        }

        #endregion

        #region ICategoryEncodingProvider Members

        public EncodingData GetEncoding(string platform, uint version)
        {
            if (Encodings.Length <= version)
                throw new InvalidOperationException($"{platform}: Encoding {version} out of range");
            return Encodings[version];
        }

        #endregion

        #region Encodings

        private readonly Lazy<EncodingData[]> _encodings;

        private EncodingData[] Encodings => _encodings.Value;

        private EncodingData[] GetEncodings()
        {
            var offsets = BootProvider.GetOffsets("PS");
            if (offsets == null)
                throw new InvalidOperationException("PS: Null offsets");
            var encodings = new EncodingData[offsets.Length + 1];
            encodings[0] = EncodingData.Empty;
            for (int i = 0; i < offsets.Length; i++)
                encodings[i + 1] = GetEncoding(offsets[i]);
            return encodings;
        }

        private static EncodingData GetEncoding(int[] offsets)
        {
            return new EncodingData
            {
                Name = "dancingbits",
                Data = GetOffsets(offsets)
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
