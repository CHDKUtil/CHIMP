using Chimp.Model;

namespace Chimp
{
    interface ILicenseProvider
    {
        LicenseData[]? GetLicenses();
    }
}
