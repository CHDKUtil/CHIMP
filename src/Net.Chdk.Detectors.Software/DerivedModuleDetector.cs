using Net.Chdk.Model.Software;
using System.Collections.Generic;

namespace Net.Chdk.Detectors.Software
{
    sealed class DerivedModuleDetector : IInnerModuleDetector
    {
        public ModuleInfo GetModule(SoftwareInfo software, byte[] buffer, string hashName)
        {
            return new ModuleInfo
            {
                Created = software.Product.Created,
                Changeset = software.Build?.Changeset,
                Hash = GetHash(hashName),
            };
        }

        private static SoftwareHashInfo GetHash(string hashName)
        {
            return new SoftwareHashInfo
            {
                Name = hashName,
                Values = new Dictionary<string, string>(),
            };
        }
    }
}
