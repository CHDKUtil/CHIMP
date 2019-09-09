using Chimp.Logging;
using System;
using System.Runtime.CompilerServices;

namespace Net.Chdk.Encoders.Binary
{
    abstract class BinaryEncoderDecoder
    {
        protected const int OffsetLength = 8;
        protected const int OffsetShift = 2;
        protected const int ChunkSize = 0x400;

        protected ILogger Logger { get; }

        public BinaryEncoderDecoder(ILogger logger)
        {
            Logger = logger;
        }

        protected const string FileName = "DISKBOOT.BIN";

        protected static void Validate(byte[] decBuffer, byte[] encBuffer, uint offsets)
        {
            if (decBuffer == null)
                throw new ArgumentNullException(nameof(decBuffer));
            if (encBuffer == null)
                throw new ArgumentNullException(nameof(encBuffer));
            Validate(offsets);
        }

        private static void Validate(uint? offsets)
        {
            if (offsets == null)
                return;
            var value = offsets.Value;
            for (var i = 0; i < OffsetLength; i++)
            {
                if ((value & 0x0f) > 7)
                    throw new ArgumentOutOfRangeException(nameof(offsets));
                value >>= (1 << OffsetShift);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static byte Dance(byte input, int index)
        {
            if ((index % 3) != 0)
                return (byte)(input ^ 0xff);
            if ((index % 2) == 0)
                return (byte)(input ^ 0xa0);
            return (byte)((byte)(input >> 4) | (byte)(input << 4));
        }
    }
}
