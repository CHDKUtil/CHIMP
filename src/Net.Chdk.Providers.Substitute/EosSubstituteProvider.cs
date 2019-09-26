using Microsoft.Extensions.Logging;

namespace Net.Chdk.Providers.Substitute
{
    sealed class EosSubstituteProvider : CategorySubstituteProvider
    {
        public EosSubstituteProvider(ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<EosSubstituteProvider>())
        {
        }

        protected override string CategoryName => "EOS";
    }
}
