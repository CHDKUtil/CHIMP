using Chimp.Model;
using Microsoft.Extensions.Options;

namespace Chimp.Providers
{
    sealed class LicenseProvider : ILicenseProvider
    {
        #region Fields

        private LicensesData LicensesData { get; }

        #endregion

        #region Constructor

        public LicenseProvider(IOptions<LicensesData> options)
        {
            LicensesData = options.Value;
        }

        #endregion

        #region ILicenseProvider Members

        public LicenseData[] GetLicenses()
        {
            return LicensesData.Licenses;
        }

        #endregion
    }
}
