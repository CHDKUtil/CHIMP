namespace Chimp.Model
{
    sealed class LicenseData
    {
        public string Product { get; set; }
        public string[] Contents { get; set; }
    }

    sealed class LicensesData
    {
        public LicenseData[] Licenses { get; set; }
    }
}
