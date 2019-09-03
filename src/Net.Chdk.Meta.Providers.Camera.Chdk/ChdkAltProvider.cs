using Net.Chdk.Meta.Model.CameraTree;
using Net.Chdk.Meta.Providers.Camera.Ps;
using System;

namespace Net.Chdk.Meta.Providers.Camera.Chdk
{
    sealed class ChdkAltProvider : ProductAltProvider
    {
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

            switch (tree.Default)
            {
                case null:
                    return tree.Names[0];
                case "KEY_PRINT":
                    return "Print";
                case "KEY_PLAYBACK":
                    return "Playback";
                case "KEY_FACE":
                    return "Face";
                default:
                    throw new InvalidOperationException($"{platform}: Unknown button {tree.Default}");
            }
        }

        protected override string[] GetAltButtons(string platform, TreeAltData tree)
        {
            return tree.Names;
        }
    }
}
