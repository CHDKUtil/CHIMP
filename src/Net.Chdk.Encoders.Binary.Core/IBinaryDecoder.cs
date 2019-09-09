namespace Net.Chdk.Encoders.Binary
{
    public interface IBinaryDecoder
    {
        bool ValidatePrefix(byte[] encBuffer, int size, byte[] prefix);
        void Decode(byte[] encBuffer, byte[] decBuffer, uint offsets);
    }
}
