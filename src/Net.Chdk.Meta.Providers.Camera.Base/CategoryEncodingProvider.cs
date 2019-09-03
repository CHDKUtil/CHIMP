using Net.Chdk.Meta.Model.Camera;
using System;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class CategoryEncodingProvider : ICategoryEncodingProvider
    {
        #region Constructor

        protected CategoryEncodingProvider()
        {
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

        public abstract string CategoryName { get; }

        #endregion

        #region Encodings

        private readonly Lazy<EncodingData[]> _encodings;

        private EncodingData[] Encodings => _encodings.Value;

        protected abstract EncodingData[] GetEncodings();

        #endregion
    }
}
