namespace Net.Chdk.Meta.Providers.Camera.Fhp
{
    sealed class FhpCameraBootProvider : ProductCameraBootProvider
    {
        public override string ProductName => "400plus";

        protected override string GetBootFileSystem(uint modelId) => "FAT32";
    }
}
