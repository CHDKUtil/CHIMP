using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Providers.Camera.Ps;
using System;
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

        protected override void Validate(string platform, TreeAltData tree)
        {
            if (tree.Adjustable)
            {
                if ((tree.Names == null || tree.Options == null))
                    throw new InvalidOperationException($"{platform}: Mismatching button options");

                if (tree.Names.Length != tree.Options.Length)
                    throw new InvalidOperationException($"{platform}: Mismatching button options");
            }
            else
            {
                if (tree.Names != null || tree.Options != null)
                    throw new InvalidOperationException($"{platform}: Invalid button options");
            }

            if (tree.Default != null && tree.Options != null && tree.Default != tree.Options[0])
                throw new InvalidOperationException($"{platform}: Mismatching buttons");
        }

        protected override string GetAltButton(string platform, TreeAltData tree)
        {
            if (!tree.Adjustable)
                return "Print";
            return GetAltButton(platform, tree, 0);
        }

        protected override string[] GetAltButtons(string platform, TreeAltData tree)
        {
            if (!tree.Adjustable)
                return null;
            var names = new string[tree.Names.Length];
            for (int i = 0; i < names.Length; i++)
                names[i] = GetAltButton(platform, tree, i);
            return names;
        }

        private string GetAltButton(string platform, TreeAltData tree, int index)
        {
            var name = tree.Names[index];
            if (ButtonNames.TryGetValue(name, out string name2))
            {
                Logger.LogWarning("{0}: {1} should be {1}", platform, name, name2);
                name = name2;
            }
            return name;
        }
    }
}
