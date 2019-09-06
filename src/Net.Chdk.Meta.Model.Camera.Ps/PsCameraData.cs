namespace Net.Chdk.Meta.Model.Camera.Ps
{
    public sealed class PsCameraData : CameraData<PsCameraData, PsCameraModelData, RevisionData, PsCardData>
    {
        public AltData Alt { get; set; }
    }
}
