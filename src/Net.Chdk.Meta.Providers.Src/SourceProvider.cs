using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model;

namespace Net.Chdk.Meta.Providers.Src
{
    public abstract class SourceProvider<T> : MakefileParsingProvider<PlatformSourceData>
    {
        protected SourceProvider(ILogger logger)
            : base(logger)
        {
        }

        public PlatformSourceData? GetSource(string platformPath, string platform, string? revision = null)
        {
            return GetValue(platformPath, platform, revision);
        }

        protected override string Prefix => "override";

        protected override void UpdateValue(ref PlatformSourceData? value, string line, string platform)
        {
            var split = line.Split('=');
            switch (split[0])
            {
                case "PLATFORM":
                    value ??= new PlatformSourceData();
                    value.Platform = split[1];
                    break;
                case "PLATFORMSUB":
                    value ??= new PlatformSourceData();
                    value.Revision = split[1];
                    break;
                default:
                    break;
            }
        }
    }
}
