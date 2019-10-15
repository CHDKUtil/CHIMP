using System.IO;
using Net.Chdk;
using Net.Chdk.Json;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;

namespace Chimp.Providers.Software
{
    sealed class ScriptSoftwareProvider : ISoftwareProvider
    {
        private string ProductName { get; }

        public ScriptSoftwareProvider(SoftwareSourceInfo source)
        {
            ProductName = source.Name;
        }

        public SoftwareInfo GetSoftware(IMatchData _, SoftwareInfo softwareInfo)
        {
            var software = GetSoftware();
            software.Camera = softwareInfo.Camera;
            software.Model = softwareInfo.Model;
            return software;
        }

        private SoftwareInfo GetSoftware()
        {
            var filePath = Path.Combine(Directories.Data, Directories.Product, ProductName, "software.json");
            using (var stream = File.OpenRead(filePath))
            {
                return JsonObject.Deserialize<SoftwareInfo>(stream);
            }
        }
    }
}
