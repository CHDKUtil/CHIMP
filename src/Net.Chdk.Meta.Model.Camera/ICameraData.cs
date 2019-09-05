namespace Net.Chdk.Meta.Model.Camera
{
    public interface ICameraData
    {
        ICameraModelData[] Models { get; }
        EncodingData Encoding { get; set; }
        CardData Card { get; }
        BootData Boot { get; }
    }
}
