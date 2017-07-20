namespace Chimp.Model
{
    public enum PartitionType : byte
    {
        None = 0x00,
        PrimaryFAT = 0x01,
        PrimaryFAT_0 = 0x06,
        ExFAT = 0x07,
        PrimaryFAT32 = 0x0B,
        PrimaryFAT32_2 = 0x0C,
    }
}
