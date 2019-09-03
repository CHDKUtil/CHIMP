using Net.Chdk.Meta.Model.Camera;

namespace Net.Chdk.Meta.Providers.Camera.Eos
{
    sealed class EosEncodingProvider : CategoryEncodingProvider
    {
        #region ICategoryEncodingProvider Members

        public override string CategoryName => "EOS";

        #endregion

        #region Encodings

        protected override EncodingData[] GetEncodings()
        {
            return new[]
            {
                EncodingData.Empty
            };
        }

        #endregion
    }
}
