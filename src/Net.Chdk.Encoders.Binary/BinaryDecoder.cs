using Chimp.Logging;

namespace Net.Chdk.Encoders.Binary
{
    sealed class BinaryDecoder : BinaryEncoderDecoder, IBinaryDecoder
    {
        public BinaryDecoder(ILoggerFactory loggerFactory)
            : base(loggerFactory.CreateLogger<BinaryDecoder>())
        {
        }

        public bool ValidatePrefix(byte[] encBuffer, int size, byte[]? prefix)
        {
            if (prefix == null || size < prefix.Length)
                return false;
            for (var i = 0; i < prefix.Length; i++)
                if (encBuffer[i] != prefix[i])
                    return false;
            return true;
        }

        public void Decode(byte[] encBuffer, byte[] decBuffer, uint offsets)
        {
            Validate(encBuffer: encBuffer, decBuffer: decBuffer, offsets: offsets);

            Logger.Log(LogLevel.Trace, "Decoding {0} with 0x{1:x}", FileName, offsets);

            for (var index = 0; index < decBuffer.Length; index++)
            {
                var offset = (int)(offsets >> ((index % OffsetLength) << OffsetShift) & (OffsetLength - 1));
                decBuffer[index] = Dance(encBuffer[(index & ~(OffsetLength - 1)) + offset], index % ChunkSize);
            }
        }
    }
}
