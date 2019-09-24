namespace Net.Chdk.Model.CameraModel
{
    public sealed class CameraModelInfo
    {
        public CameraModelInfo(string[]? names)
        {
            Names = names;
        }

        public string[]? Names { get; set; }
    }
}
