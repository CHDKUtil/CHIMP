using System;
using System.Security.Cryptography;
using System.Text;

namespace Net.Chdk.Providers.Crypto
{
    sealed class HashProvider : IHashProvider
    {
        public string GetHashString(byte[] buffer, string hashName)
        {
            var hash = ComputeHash(buffer, hashName);
            return GetHashString(hash);
        }

        private static string GetHashString(byte[] hash)
        {
            var sb = new StringBuilder(hash.Length * 2);
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));
            return sb.ToString();
        }

        private static byte[] ComputeHash(byte[] buffer, string hashName)
        {
            using var hash = CreateHashAlgorithm(hashName);
            return hash.ComputeHash(buffer);
        }

        private static HashAlgorithm CreateHashAlgorithm(string hashName)
        {
            return hashName switch
            {
                "md5" => MD5.Create(),
                "sha1" => SHA1.Create(),
                "sha256" => SHA256.Create(),
                "sha384" => SHA384.Create(),
                "sha512" => SHA512.Create(),
                _ => throw new ArgumentException("Invalid hash name"),
            };
        }
    }
}
