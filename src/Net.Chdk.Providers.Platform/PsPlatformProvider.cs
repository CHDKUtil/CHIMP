using Microsoft.Extensions.Logging;

namespace Net.Chdk.Providers.Platform
{
    sealed class PsPlatformProvider : CategoryPlatformProvider
    {
        public PsPlatformProvider(ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<PsPlatformProvider>())
        {
        }

        protected override string CategoryName => "PS";
    }
}
