namespace Net.Chdk.Meta.Providers.Camera.Ml
{
    sealed class MlCameraBootProvider : ProductCameraBootProvider
    {
        public override string ProductName => "ML";

        protected override string GetBootFileSystem(uint modelId) => "exFAT";
    }
}
