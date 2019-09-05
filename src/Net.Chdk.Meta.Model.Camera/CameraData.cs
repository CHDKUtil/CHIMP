using System.Linq;

namespace Net.Chdk.Meta.Model.Camera
{
    public abstract class CameraData<TCamera, TModel, TCard> : ICameraData
        where TCamera : CameraData<TCamera, TModel, TCard>
        where TModel : CameraModelData
        where TCard : CardData
    {
        public TModel[] Models { get; set; }
        public EncodingData Encoding { get; set; }
        public TCard Card { get; set; }
        public BootData Boot { get; set; }

        ICameraModelData[] ICameraData.Models
        {
            get => Models;
            set => Models = value?.Cast<TModel>().ToArray();
        }

        CardData ICameraData.Card => Card;
    }
}
