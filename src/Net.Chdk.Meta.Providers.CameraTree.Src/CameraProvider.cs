using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraTree;
using System;
using System.Linq;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    sealed class CameraProvider : HeaderParsingProvider<CameraData>
    {
        public CameraProvider(ILogger<CameraProvider> logger)
            : base(logger)
        {
        }

        public CameraData GetCamera(string platformPath, string platform)
        {
            return GetValue(platformPath, platform, null);
        }

        protected override string FileName => "platform_camera.h";

        protected override void UpdateValue(ref CameraData camera, string line, string platform)
        {
            var split = line.Split();
            switch (split[0])
            {
                case "CAM_ADJUSTABLE_ALT_BUTTON":
                    GetAlt(ref camera).Adjustable = GetBoolean(split, platform);
                    break;
                case "CAM_DEFAULT_ALT_BUTTON":
                    GetAlt(ref camera).Default = GetAltButtonDefault(split, platform);
                    break;
                case "CAM_ALT_BUTTON_OPTIONS":
                    GetAlt(ref camera).Options = GetAltButtonOptions(split, platform);
                    break;
                case "CAM_ALT_BUTTON_NAMES":
                    GetAlt(ref camera).Names = GetAltButtonNames(split, platform);
                    break;
                case "CAM_MULTIPART":
                    GetCard(ref camera).Multi = GetBoolean(split, platform);
                    break;
                default:
                    break;
            }
        }

        private static TreeAltData GetAlt(ref CameraData camera)
        {
            return GetCamera(ref camera).Alt;
        }

        private static TreeCardData GetCard(ref CameraData camera)
        {
            return GetCamera(ref camera).Card;
        }

        private static CameraData GetCamera(ref CameraData camera)
        {
            return camera ?? (camera = new CameraData());
        }

        private bool GetBoolean(string[] split, string platform)
        {
            var value = split[split.Length - 1];
            if (!"1".Equals(value))
            {
                var name = GetName(platform);
                throw new InvalidOperationException($"{name}: Unexpected value {value}");
            }
            return true;
        }

        private static string GetAltButtonDefault(string[] split, string platform)
        {
            return split[split.Length - 1];
        }

        private string[] GetAltButtonOptions(string[] split, string platform)
        {
            return ParseArray(split, platform);
        }

        private string[] GetAltButtonNames(string[] split, string platform)
        {
            split = ParseArray(split, platform);

            for (int i = 0; i < split.Length; ++i)
                split[i] = TrimQuotes(split[i], platform);

            return split;
        }

        private string[] ParseArray(string[] split, string platform)
        {
            var skip = split.SkipWhile(s => s.Length == 0 || s[0] != '{');
            var concat = string.Concat(skip);
            var trim = TrimBraces(concat, platform);
            var split2 = trim.Split(',');
            return TrimEnd(split2, platform);
        }

        private string[] TrimEnd(string[] split, string platform)
        {
            int index = split.Length - 1;
            while (index >= 0 && split[index].Length == 0)
                --index;

            if (index < split.Length - 1)
            {
                if (index == 0)
                {
                    var name = GetName(platform);
                    throw new InvalidOperationException($"{name}: Empty array");
                }
                Array.Resize(ref split, index + 1);
            }

            return split;
        }

        private static string TrimBraces(string str, string platform)
        {
            var first = str[0];
            if (first != '{')
                throw new InvalidOperationException($"Unexpected character {first}");

            str = str.Substring(1).TrimStart();

            var last = str[str.Length - 1];
            if (last != '}')
                throw new InvalidOperationException($"Unexpected character {last}");

            return str.Substring(0, str.Length - 1).TrimEnd();
        }

        private static string TrimQuotes(string str, string platform)
        {
            var first = str[0];
            if (first != '"')
                throw new InvalidOperationException($"Unexpected character {first}");

            str = str.Substring(1);

            var last = str[str.Length - 1];
            if (last != '"')
                throw new InvalidOperationException($"Unexpected character {last}");

            return str.Substring(0, str.Length - 1);
        }
    }
}
