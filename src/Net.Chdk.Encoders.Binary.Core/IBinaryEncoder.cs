namespace Net.Chdk.Encoders.Binary
{
    public interface IBinaryEncoder
    {
        void Encode(byte[] decBuffer, byte[] encBuffer, uint offsets);
    }
}
