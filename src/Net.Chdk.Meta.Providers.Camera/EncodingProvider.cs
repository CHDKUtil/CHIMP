using Net.Chdk.Meta.Model.Camera;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    sealed class EncodingProvider : SingleCategoryProvider<ICategoryEncodingProvider>, IEncodingProvider
    {
        public EncodingProvider(IEnumerable<ICategoryEncodingProvider> innerProviders)
            : base(innerProviders)
        {
        }

        public EncodingData GetEncoding(string platform, uint version, string categoryName)
        {
            return GetInnerProvider(categoryName).GetEncoding(platform, version);
        }
    }
}
