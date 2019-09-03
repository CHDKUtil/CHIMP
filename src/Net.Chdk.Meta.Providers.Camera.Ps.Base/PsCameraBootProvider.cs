namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public abstract class PsCameraBootProvider : ProductCameraBootProvider
    {
        private const uint MinModelId = 0x1010000;
        private const uint MinFat32ModelId = 0x2980000;

        protected override string GetBootFileSystem(uint modelId)
        {
            return modelId < MinModelId || modelId >= MinFat32ModelId
                ? "FAT32"
                : "FAT";
        }
    }
}
