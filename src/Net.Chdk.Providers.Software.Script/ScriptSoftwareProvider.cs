using System;
using System.IO;
using Net.Chdk.Json;
using Net.Chdk.Model.Software;

namespace Net.Chdk.Providers.Software.Script
{
    public sealed class ScriptSoftwareProvider : ISoftwareProvider<ScriptMatchData>
    {
        private string ProductName { get; }

        public ScriptSoftwareProvider(SoftwareSourceInfo source)
        {
            if (source.Name == null)
                throw new InvalidOperationException("Null product name");
            ProductName = source.Name;
        }

        public SoftwareInfo GetSoftware(ScriptMatchData? _, SoftwareInfo softwareInfo)
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
