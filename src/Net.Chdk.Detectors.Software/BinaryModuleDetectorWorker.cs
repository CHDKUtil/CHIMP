using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Detectors.Software
{
    sealed class BinaryModuleDetectorWorker : BinaryDetectorWorker<ModuleInfo, IProductBinaryModuleDetector>
    {
        public BinaryModuleDetectorWorker(IEnumerable<IProductBinaryModuleDetector> detectors)
            : base(detectors)
        {
        }

        public ModuleInfo GetModule(SoftwareInfo software, byte[] buffer, string hashName)
        {
            var tuples = Detectors
                .Select(d => GetBytes(d, software, hashName))
                .ToArray();
            return GetValue(buffer, tuples);
        }

        private static Tuple<Func<byte[], int, ModuleInfo>, byte[]> GetBytes(IProductBinaryModuleDetector detector, SoftwareInfo software, string hashName)
        {
            return Tuple.Create<Func<byte[], int, ModuleInfo>, byte[]>((b, i) => detector.GetModule(software, b, i, hashName), detector.Bytes);
        }
    }
}
