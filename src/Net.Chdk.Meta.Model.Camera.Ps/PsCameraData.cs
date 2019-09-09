namespace Net.Chdk.Meta.Model.Camera.Ps
{
    public sealed class PsCameraData : CameraData<PsCameraData, PsCameraModelData, RevisionData, PsCardData>
    {
        public EncodingData Encoding { get; set; }
        public AltData Alt { get; set; }
    }
}
