using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Providers.Camera.Ps;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera.Chdk
{
    sealed class ChdkAltProvider : ProductAltProvider
    {
        private static readonly Dictionary<string, string> ButtonNames = new Dictionary<string, string>
        {
            { "Play", "Playback" },
            { "Disp", "Display" },
            { "Shortcut", "Shrtcut" },
            { "VIDEO", "Video" },
            { "WiFi", "Wifi" },
        };

        public ChdkAltProvider(ILogger<ChdkAltProvider> logger)
            : base(logger)
        {
        }

        public override string ProductName => "CHDK";

        protected override string GetAltButton(string platform, string[]? altNames)
        {
            if (altNames == null)
                return "Print";
            return GetAltButton(platform, altNames, 0);
        }

        protected override string[]? GetAltButtons(string platform, string[]? altNames)
        {
            if (altNames == null)
                return null;
            var names = new string[altNames.Length];
            for (int i = 0; i < names.Length; i++)
                names[i] = GetAltButton(platform, altNames, i);
            return names;
        }

        private string GetAltButton(string platform, string[] altNames, int index)
        {
            var name = altNames[index];
            if (ButtonNames.TryGetValue(name, out string name2))
            {
                Logger.LogWarning("{0}: {1} should be {1}", platform, name, name2);
                name = name2;
            }
            return name;
        }
    }
}
