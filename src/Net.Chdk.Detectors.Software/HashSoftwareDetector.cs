using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Net.Chdk.Encoders.Binary;
using Net.Chdk.Json;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Boot;
using Net.Chdk.Providers.Camera;
using Net.Chdk.Providers.Software;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    sealed class BytesComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[] x, byte[] y)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
        }

        public int GetHashCode(byte[] obj)
        {
            return StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
        }
    }

    abstract class HashSoftwareDetector : BinarySoftwareDetectorBase
    {
        protected HashSoftwareDetector(IEnumerable<IProductBinarySoftwareDetector> softwareDetectors, IBinaryDecoder binaryDecoder, IBootProvider bootProvider, ICameraProvider cameraProvider, ISoftwareHashProvider hashProvider, IOptions<SoftwareDetectorSettings> settings, ILogger logger)
            : base(softwareDetectors, binaryDecoder, bootProvider, cameraProvider, hashProvider, settings, logger)
        {
            _hash2software = new Lazy<IDictionary<byte[], SoftwareInfo>>(GetHash2Software);
        }

        public override SoftwareInfo GetSoftware(byte[] inBuffer, IProgress<double> progress, CancellationToken token)
        {
            var fileName = BootProvider.GetFileName(CategoryName);
            var hash = HashProvider.GetHash(inBuffer, fileName, HashName);
            var hashStr = hash.Values[fileName.ToLowerInvariant()];
            var hashBytes = GetHashBytes(hashStr);
            Hash2Software.TryGetValue(hashBytes, out SoftwareInfo software);
            return software;
        }

        public override bool UpdateSoftware(SoftwareInfo software, byte[] inBuffer)
        {
            if (!CategoryName.Equals(software.Category.Name, StringComparison.Ordinal))
                return false;

            var software2 = GetSoftware(inBuffer, null, default(CancellationToken));
            if (software2 != null)
            {
                software.Hash = software2.Hash;
                software.Product = software2.Product;
                software.Build = software2.Build;
                software.Compiler = software2.Compiler;
                software.Encoding = software2.Encoding;
                return true;
            }

            return false;
        }

        private readonly Lazy<IDictionary<byte[], SoftwareInfo>> _hash2software;

        private IDictionary<byte[], SoftwareInfo> Hash2Software => _hash2software.Value;

        private IDictionary<byte[], SoftwareInfo> GetHash2Software()
        {
            var path = Path.Combine(Directories.Data, Directories.Category, CategoryName, "hash2sw.json");
            IDictionary<string, SoftwareInfo> hash2sw;
            using (var stream = File.OpenRead(path))
            {
                hash2sw = JsonObject.Deserialize<IDictionary<string, SoftwareInfo>>(stream);
            }
            var result = new Dictionary<byte[], SoftwareInfo>(new BytesComparer());
            foreach (var kvp in hash2sw)
            {
                var bytes = GetHashBytes(kvp.Key);
                result.Add(bytes, kvp.Value);
            }
            return result;
        }

        private byte[] GetHashBytes(string hashStr)
        {
            var bytes = new byte[hashStr.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hashStr.Substring(i * 2, 2), 16);
            return bytes;
        }

        protected override uint?[] GetOffsets()
        {
            throw new NotImplementedException();
        }
    }
}
