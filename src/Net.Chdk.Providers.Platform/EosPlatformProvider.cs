using Microsoft.Extensions.Logging;

namespace Net.Chdk.Providers.Platform
{
    sealed class EosPlatformProvider : CategoryPlatformProvider
    {
        public EosPlatformProvider(ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<EosPlatformProvider>())
        {
        }

        protected override string CategoryName => "EOS";
    }
}
