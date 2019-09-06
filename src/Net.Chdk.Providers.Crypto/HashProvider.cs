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
            return CreateHashAlgorithm(hashName)
                .ComputeHash(buffer);
        }

        private static HashAlgorithm CreateHashAlgorithm(string hashName)
        {
            switch (hashName)
            {
                case "md5":
                    return MD5.Create();
                case "sha1":
                    return SHA1.Create();
                case "sha256":
                    return SHA256.Create();
                case "sha384":
                    return SHA384.Create();
                case "sha512":
                    return SHA512.Create();
                default:
                    throw new ArgumentException("Invalid hash name");
            }
        }
    }
}
