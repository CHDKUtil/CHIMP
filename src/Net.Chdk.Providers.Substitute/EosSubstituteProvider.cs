using Microsoft.Extensions.Logging;
using Net.Chdk.Generators.Platform;
using Net.Chdk.Providers.Firmware;

namespace Net.Chdk.Providers.Substitute
{
    sealed class EosSubstituteProvider : CategorySubstituteProvider
    {
        public EosSubstituteProvider(IPlatformGenerator platformGenerator, IFirmwareProvider firmwareProvider, ILoggerFactory loggerFactory)
            : base(platformGenerator, firmwareProvider, loggerFactory.CreateLogger<EosSubstituteProvider>())
        {
        }

        protected override string CategoryName => "EOS";
    }
}
