using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Platform;
using Net.Chdk.Providers.Firmware;

namespace Net.Chdk.Providers.Substitute
{
    sealed class PsSubstituteProvider : CategorySubstituteProvider
    {
        public PsSubstituteProvider(IPlatformGenerator platformGenerator, IFirmwareProvider firmwareProvider, ILoggerFactory loggerFactory)
            : base(platformGenerator, firmwareProvider, loggerFactory.CreateLogger<PsSubstituteProvider>())
        {
        }

        protected override string CategoryName => "PS";
    }
}
