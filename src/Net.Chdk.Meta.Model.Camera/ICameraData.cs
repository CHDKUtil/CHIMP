namespace Net.Chdk.Meta.Model.Camera
{
    public interface ICameraData
    {
        ICameraModelData[] Models { get; set; }
        CardData Card { get; }
        BootData Boot { get; }
    }

    public interface ICameraData<TCamera, TModel> : ICameraData
        where TCamera : ICameraData<TCamera, TModel>
        where TModel : ICameraModelData
    {
    }
}
