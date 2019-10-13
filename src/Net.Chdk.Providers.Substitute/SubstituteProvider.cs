using Microsoft.Extensions.Logging;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Firmware;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Providers.Substitute
{
    sealed class SubstituteProvider : ProviderResolver<ICategorySubstituteProvider>, ISubstituteProvider
    {
        private IFirmwareProvider FirmwareProvider { get; }
        private ILogger Logger { get; }

        public SubstituteProvider(IFirmwareProvider firmwareProvider, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            FirmwareProvider = firmwareProvider;
            Logger = loggerFactory.CreateLogger<SubstituteProvider>();
        }

        public IDictionary<string, object>? GetSubstitutes(SoftwareInfo software)
        {
            var categoryName = FirmwareProvider.GetCategoryName(software.Camera);
            if (categoryName == null)
                return null;

            return GetProvider(categoryName)?
                .GetSubstitutes(software);
        }

        public IEnumerable<string> GetSupportedPlatforms(SoftwareInfo software)
        {
            return Providers.Values
                .SelectMany(p => p.GetSupportedPlatforms(software));
        }

        public IEnumerable<string> GetSupportedRevisions(SoftwareInfo software)
        {
            return Providers.Values
                .SelectMany(p => p.GetSupportedRevisions(software));
        }

        protected override IEnumerable<string> GetNames()
        {
            return new[] { "EOS", "PS" };
        }

        protected override ICategorySubstituteProvider CreateProvider(string categoryName)
        {
            return new CategorySubstituteProvider(FirmwareProvider, categoryName, Logger);
        }
    }
}
