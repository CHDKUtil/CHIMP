using Net.Chdk.Encoders.Binary;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Net.Chdk.Detectors.Software
{
    sealed class BinarySoftwareDetectorWorker : BinaryDetectorWorker<SoftwareInfo, IProductBinarySoftwareDetector>
    {
        private const string EncodingName = "dancingbits";

        private readonly byte[] inBuffer;
        private readonly byte[] encBuffer;
        private readonly byte[] decBuffer;
        private readonly Action<uint?> decode;
        private readonly uint?[] offsets;

        public BinarySoftwareDetectorWorker(IEnumerable<IProductBinarySoftwareDetector> detectors, IBinaryDecoder binaryDecoder, byte[] prefix, byte[] inBuffer, int startIndex, int endIndex, uint?[] offsets)
            : base(detectors)
        {
            this.inBuffer = inBuffer;
            if (offsets != null)
            {
                var prefixLength = prefix != null
                    ? prefix.Length
                    : 0;
                this.encBuffer = new byte[inBuffer.Length - prefixLength];
                Array.Copy(inBuffer, prefixLength, encBuffer, 0, encBuffer.Length);
                this.decBuffer = new byte[encBuffer.Length];
                this.decode = o => binaryDecoder.Decode(encBuffer, decBuffer, o.Value);

                this.offsets = new uint?[endIndex - startIndex];
                for (var i = 0; i < this.offsets.Length; i++)
                    this.offsets[i] = offsets[i + startIndex];
            }
        }

        public BinarySoftwareDetectorWorker(IEnumerable<IProductBinarySoftwareDetector> detectors, IBinaryDecoder binaryDecoder, byte[] prefix, byte[] inBuffer, SoftwareEncodingInfo encoding)
            : this(detectors, binaryDecoder, prefix, inBuffer, 0, 1, encoding != null ? new[] { encoding.Data } : null)
        {
        }

        public SoftwareInfo GetSoftware(ProgressState progress, CancellationToken token)
        {
            if (offsets == null)
                return PlainGetSoftware();

            for (var index = 0; index < offsets.Length; index++)
            {
                if (progress.IsCompleted)
                    return null;
                token.ThrowIfCancellationRequested();
                var software = GetSoftware(offsets[index]);
                if (software != null)
                {
                    progress.SetCompleted();
                    return software;
                }
                progress.Update();
            }

            return null;
        }

        private SoftwareInfo GetSoftware(uint? offsets)
        {
            var buffer = Decode(offsets);
            if (buffer == null)
                return null;
            var software = GetSoftware(buffer);
            if (software != null)
                software.Encoding = GetEncoding(offsets);
            return software;
        }

        private SoftwareInfo PlainGetSoftware()
        {
            var software = GetSoftware(inBuffer);
            if (software != null)
                software.Encoding = GetEncoding(null);
            return software;
        }

        private SoftwareInfo GetSoftware(byte[] buffer)
        {
            var tuples = Detectors
                .Select(GetBytes)
                .ToArray();
            return GetValue(buffer, tuples);
        }

        private static Tuple<Func<byte[], int, SoftwareInfo>, byte[]> GetBytes(IProductBinarySoftwareDetector d)
        {
            return Tuple.Create<Func<byte[], int, SoftwareInfo>, byte[]>(d.GetSoftware, d.Bytes);
        }

        private byte[] Decode(uint? offsets)
        {
            if (offsets == null)
                return inBuffer;
            decode(offsets.Value);
            return decBuffer;
        }

        private static SoftwareEncodingInfo GetEncoding(uint? offsets)
        {
            return new SoftwareEncodingInfo
            {
                Name = offsets != null ? EncodingName : string.Empty,
                Data = offsets
            };
        }
    }
}
