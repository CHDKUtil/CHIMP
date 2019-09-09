using System.Linq;

namespace Net.Chdk.Meta.Model.Camera
{
    public abstract class CameraData<TCamera, TModel, TRevision, TCard> : ICameraData<TCamera, TModel>
        where TCamera : CameraData<TCamera, TModel, TRevision, TCard>
        where TModel : CameraModelData<TModel, TRevision>
        where TRevision : IRevisionData
        where TCard : CardData
    {
        public TModel[] Models { get; set; }
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
