using System;
using System.Text;

namespace Net.Chdk.Detectors.Software.Product
{
    public abstract class ProductBinaryDetector : ProductDetectorBase
    {
        protected ProductBinaryDetector()
        {
            bytes = new Lazy<byte[]>(GetBytes);
        }

        private readonly Lazy<byte[]> bytes;

        private byte[] GetBytes()
        {
            return Encoding.ASCII.GetBytes(String);
        }

        public byte[] Bytes => bytes.Value;

        public abstract string ProductName { get; }

        protected abstract string String { get; }

        protected abstract int StringCount { get; }

        protected virtual char SeparatorChar => '\0';

        protected static string[] GetStrings(byte[] buffer, int index, int length, char separator)
        {
            var strings = new string[length];
            for (var i = 0; i < length; i++)
                strings[i] = GetString(buffer, ref index, separator);
            return strings;
        }

        private static string GetString(byte[] buffer, ref int index, char separator)
        {
            if (index >= buffer.Length)
                return null;

            int count;
            for (count = 0; index + count < buffer.Length && buffer[index + count] != separator; count++) ;
            if (index + count == buffer.Length)
                return null;
            var str = Encoding.ASCII.GetString(buffer, index, count);
            index += count + 1;
            return str;
        }
    }
}
