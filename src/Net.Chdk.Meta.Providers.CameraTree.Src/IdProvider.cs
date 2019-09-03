using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class IdProvider : MakefileParsingProvider<TreeIdData>
    {
        public IdProvider(ILogger<IdProvider> logger)
            : base(logger)
        {
        }

        public TreeIdData GetId(string platformPath, string platform, string revision = null)
        {
            return GetValue(platformPath, platform, revision);
        }

        protected override string Prefix => string.Empty;

        protected override void UpdateValue(ref TreeIdData value, string line, string platform)
        {
            var split = line.Split('=');
            switch (split[0].Trim())
            {
                case "PLATFORMID":
                    value = value ?? GetIdValue(split[1].Trim());
                    break;
                default:
                    break;
            }
        }

        private TreeIdData GetIdValue(string version)
        {
            return new TreeIdData
            {
                Id = ushort.Parse(version)
            };
        }
    }
}
