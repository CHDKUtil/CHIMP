namespace Net.Chdk.Meta.Model.Camera
{
    public interface ICameraData
    {
        ICameraModelData[] Models { get; set; }
        EncodingData Encoding { get; set; }
        CardData Card { get; }
        BootData Boot { get; }
    }
}
