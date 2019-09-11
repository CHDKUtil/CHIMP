namespace Net.Chdk.Meta.Model.Camera
{
    public abstract class CameraData
    {
        public CameraModelData[] Models { get; set; }
        public BootData Boot { get; set; }
    }

    public abstract class CameraData<TCamera, TCard> : CameraData
        where TCamera : CameraData<TCamera, TCard>
        where TCard : CardData
    {
        public TCard Card { get; set; }
    }
}
