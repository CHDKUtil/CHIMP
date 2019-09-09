namespace Net.Chdk.Detectors.Software
{
    struct Offsets
    {
        #region Static Members

        private const int OffsetsLength = 8;

        public static Offsets Empty = new Offsets();

        public static int GetOffsetCount(int maxLength = OffsetsLength)
        {
            if (maxLength == 1)
                return 1;
            return maxLength * GetOffsetCount(maxLength - 1);
        }

        #endregion

        #region Instance Members

        private uint Value { get; }
        private int Length { get; }
        private int Mask { get; }

        public Offsets(Offsets prefix, int last)
        {
            Value = prefix.Value + ((uint)last << (prefix.Length << 2));
            Length = prefix.Length + 1;
            Mask = prefix.Mask | (1 << last);
        }

        public void GetAllOffsets(uint?[] offsets, ref int index)
        {
            if (Length == OffsetsLength)
                offsets[index++] = GetOffsets();
            else
                GetOffsets(offsets, ref index);
        }

        private void GetOffsets(uint?[] offsets, ref int index)
        {
            for (var i = 0; i < OffsetsLength; i++)
            {
                if (!Contains(i))
                {
                    var prefix2 = new Offsets(this, i);
                    prefix2.GetAllOffsets(offsets, ref index);
                }
            }
        }

        private bool Contains(int i)
        {
            return (Mask & (1 << i)) != 0;
        }

        private uint? GetOffsets()
        {
            return Value;
        }

        #endregion
    }
}
