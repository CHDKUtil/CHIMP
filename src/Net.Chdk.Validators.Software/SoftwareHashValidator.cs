using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Crypto;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;

namespace Net.Chdk.Validators.Software
{
    sealed class SoftwareHashValidator : Validator<SoftwareHashInfo>
    {
        private static readonly string[] SecureHashes = new[] { "sha256", "sha384", "sha512" };

        private IHashProvider HashProvider { get; }

        public SoftwareHashValidator(IHashProvider hashProvider)
        {
            HashProvider = hashProvider;
        }

        protected override void DoValidate(SoftwareHashInfo hash, string basePath, IProgress<double>? _, CancellationToken token)
        {
            if (string.IsNullOrEmpty(hash.Name))
                throw new ValidationException("Missing hash name");

            if (!SecureHashes.Contains(hash.Name))
                throw new ValidationException("Invalid hash name");

            if (hash.Values == null)
                throw new ValidationException("Null hash values");

            if (hash.Values.Count == 0)
                throw new ValidationException("Missing hash values");

            foreach (var kvp in hash.Values)
                Validate(kvp.Key, kvp.Value, basePath, hash.Name!, token);
        }

        private void Validate(string key, string value, string basePath, string hashName, CancellationToken token)
        {
            if (string.IsNullOrEmpty(key))
                ThrowValidationException("Missing file path");

            if (string.IsNullOrEmpty(value))
                ThrowValidationException("Missing hash value for {0}", key);

            var fileName = key.ToUpperInvariant();
            if (Path.IsPathRooted(fileName))
                ThrowValidationException("Invalid file path: {0}", key);

            var filePath = Path.Combine(basePath, fileName);
            if (!File.Exists(filePath))
                ThrowValidationException("Missing {0}", fileName);

            token.ThrowIfCancellationRequested();

            var hashString = GetHashString(filePath, hashName);
            if (!hashString.Equals(value))
                ThrowValidationException("Mismatching hash for {0}", key);
        }

        private string GetHashString(string filePath, string hashName)
        {
            var buffer = File.ReadAllBytes(filePath);
            return HashProvider.GetHashString(buffer, hashName);
        }
    }
}
