using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Net.Chdk.Detectors.Software
{
    abstract class BinaryDetectorWorker<TData, TDetector>
        where TData : class
    {
        protected IEnumerable<TDetector> Detectors { get; }

        public BinaryDetectorWorker(IEnumerable<TDetector> detectors)
        {
            Detectors = detectors;
        }

        protected static TData GetValue(byte[] buffer, Tuple<Func<byte[], int, TData>, byte[]>[] tuples)
        {
            if (tuples.Length == 0)
                return null;
            var maxLength = tuples.Max(t => t.Item2.Length);
            for (int i = 0; i < buffer.Length - maxLength; i++)
            {
                for (int j = 0; j < tuples.Length; j++)
                {
                    var bytes = tuples[j].Item2;
                    var getValue = tuples[j].Item1;
                    if (Equals(buffer, bytes, i))
                    {
                        var module = getValue(buffer, i + bytes.Length);
                        if (module != null)
                            return module;
                    }
                }
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool Equals(byte[] buffer, byte[] bytes, int start)
        {
            for (var j = 0; j < bytes.Length; j++)
                if (buffer[start + j] != bytes[j])
                    return false;
            return true;
        }
    }
}
