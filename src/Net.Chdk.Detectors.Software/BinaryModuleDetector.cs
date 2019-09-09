using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Detectors.Software
{
    sealed class BinaryModuleDetector : IInnerModuleDetector
    {
        private IEnumerable<IProductBinaryModuleDetector> Detectors { get; }

        public BinaryModuleDetector(IEnumerable<IProductBinaryModuleDetector> detectors)
        {
            Detectors = detectors;
        }

        public ModuleInfo GetModule(SoftwareInfo software, byte[] buffer, string hashName)
        {
            var detectors = GetDetectors(software);
            var worker = new BinaryModuleDetectorWorker(detectors);
            return worker.GetModule(software, buffer, hashName);
        }

        private IEnumerable<IProductBinaryModuleDetector> GetDetectors(SoftwareInfo software)
        {
            return Detectors
                .Where(d => d.ProductName.Equals(software.Product.Name, StringComparison.Ordinal));
        }
    }
}
