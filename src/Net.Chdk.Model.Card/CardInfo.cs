namespace Net.Chdk.Model.Card
{
    public sealed class CardInfo
    {
        public string DeviceId { get; set; }
        public string DriveLetter { get; set; }
        public string Label { get; set; }
        public string FileSystem { get; set; }
        public ulong? Capacity { get; set; }
        public ulong? FreeSpace { get; set; }
    }
}
