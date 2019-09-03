using Net.Chdk.Meta.Model.Camera;
using Net.Chdk.Providers.Boot;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    sealed class PsEncodingProvider : CategoryEncodingProvider
    {
        #region Fields

        private int[][] Offsets { get; }

        #endregion

        #region Constructor

        public PsEncodingProvider(IBootProvider bootProvider)
        {
            Offsets = bootProvider.GetOffsets("PS");
        }

        #endregion

        #region ICategoryEncodingProvider Members

        public override string CategoryName => "PS";

        #endregion

        #region Encodings

        protected override EncodingData[] GetEncodings()
        {
            var length = Offsets != null
                ? Offsets.Length
                : 1;
            return Enumerable.Range(0, length + 1)
                .Select(GetOffsets)
                .ToArray();
        }

        private EncodingData GetOffsets(int i)
        {
            return i > 0
                ? GetEncoding(Offsets[i - 1])
                : EncodingData.Empty;
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
