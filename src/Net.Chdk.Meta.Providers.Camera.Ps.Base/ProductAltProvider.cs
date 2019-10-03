using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.Camera.Ps;

namespace Net.Chdk.Meta.Providers.Camera.Ps
{
    public abstract class ProductAltProvider : IProductAltProvider
    {
        protected ILogger Logger { get; }

        protected ProductAltProvider(ILogger logger)
        {
            Logger = logger;
        }

        public AltData GetAlt(string platform, string[]? altNames)
        {
            return new AltData
            {
                Button = GetAltButton(platform, altNames),
                Buttons = GetAltButtons(platform, altNames),
            };
        }

        public abstract string ProductName { get; }

        protected abstract string GetAltButton(string platform, string[]? altNames);

        protected abstract string[]? GetAltButtons(string platform, string[]? altNames);
    }
}
