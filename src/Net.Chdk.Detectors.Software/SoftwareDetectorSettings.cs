namespace Net.Chdk.Detectors.Software
{
    public sealed class SoftwareDetectorSettings
    {
        public string HashName { get; set; } = "sha256";
        public bool ShuffleOffsets { get; set; } = true;
        public int MaxThreads { get; set; } = 0;
    }
}
