namespace Net.Chdk.Providers.Camera
{
    public sealed class AltInfo
    {
        public static readonly AltInfo Empty = new AltInfo();

        public string Button { get; set; }
        public string[] Buttons { get; set; }
    }
}
