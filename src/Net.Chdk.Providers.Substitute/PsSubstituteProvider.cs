using Microsoft.Extensions.Logging;

namespace Net.Chdk.Providers.Substitute
{
    sealed class PsSubstituteProvider : CategorySubstituteProvider
    {
        public PsSubstituteProvider(ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<PsSubstituteProvider>())
        {
        }

        protected override string CategoryName => "PS";
    }
}
