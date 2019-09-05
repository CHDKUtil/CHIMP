using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Model.CameraTree;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public abstract class ProductAltProvider : IProductAltProvider
    {
        protected ILogger Logger { get; }

        protected ProductAltProvider(ILogger logger)
        {
            Logger = logger;
        }

        public AltData GetAlt(string platform, TreeAltData tree)
        {
            Validate(platform, tree);


            return new AltData
            {
                Button = GetAltButton(platform, tree),
                Buttons = GetAltButtons(platform, tree),
            };
        }

        public abstract string ProductName { get; }

        protected virtual void Validate(string platform, TreeAltData tree)
        {
            //Do nothing
        }

        protected virtual string[] GetAltButtons(string platform, TreeAltData tree)
        {
            return null;
        }

        protected abstract string GetAltButton(string platform, TreeAltData tree);
    }
}
