using Net.Chdk.Generators.Platform;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Net.Chdk.Meta.Providers.Platform
{
    sealed class PlatformProvider : SingleExtensionProvider<IInnerPlatformProvider>, IPlatformProvider
    {
        private IPlatformGenerator PlatformGenerator { get; }

        public PlatformProvider(IEnumerable<IInnerPlatformProvider> innerProviders, IPlatformGenerator platformGenerator)
            : base(innerProviders)
        {
            PlatformGenerator = platformGenerator;
        }

        public IDictionary<string, CameraModel[]> GetPlatforms(string path, string? category)
        {
            var provider = GetInnerProvider(path, out string ext);
            if (provider == null)
                throw new InvalidOperationException($"Unknown platform extension: {ext}");

            using var reader = File.OpenText(path);

            var keys = provider.GetPlatforms(reader);
            return GetPlatforms(keys, category);
        }

        private IDictionary<string, CameraModel[]> GetPlatforms(IEnumerable<KeyValuePair<string, string>> values, string? category)
        {
            var platforms = new SortedDictionary<string, CameraModel[]>();
            foreach (var kvp in values)
            {
                var modelId = uint.Parse(kvp.Key.TrimStart("0x"), NumberStyles.HexNumber);
                var models = GetCameraModels(modelId, kvp.Value, category).ToArray();
                if (models[0] != null)
                {
                    var key = $"0x{modelId:x07}";
                    if (!platforms.TryGetValue(key, out CameraModel?[] value))
                        value = models.ToArray();
                    else
                        value = value.Concat(models).ToArray();
                    platforms[key] = value!;
                }
            }
            return platforms;
        }

        private IEnumerable<CameraModel?> GetCameraModels(uint modelId, string value, string? category)
        {
            var models = GetModels(value)
                .Select(m => m.TrimEnd(" (new)"))
                .ToArray();

            return GetModelMatrix(models)
                .Select(n => GetCameraModel(modelId, n, category));
        }

        private CameraModel? GetCameraModel(uint modelId, string[] names, string? category)
        {
            var platform = GetPlatform(modelId, names, category);
            if (platform == null)
                return null;

            return new CameraModel
            {
                Names = GetNames(names).ToArray(),
                Platform = platform
            };
        }

        private string? GetPlatform(uint modelId, string[] names, string? category)
        {
            return PlatformGenerator.GetPlatform(modelId, names, category);
        }

        private static IEnumerable<string> GetNames(string[] names)
        {
            return names[0].StartsWith("EOS ")
                ? names.Select(PrependCanonEos)
                : names.Select(PrependCanon);
        }

        private static string PrependCanon(string name)
        {
            return $"Canon {name}";
        }

        private static string PrependCanonEos(string name)
        {
            return $"Canon EOS {name.TrimStart("EOS ")}";
        }

        private static IEnumerable<string> GetModels(string value)
        {
            int index;
            var startIndex = 0;
            while ((index = value.IndexOf(" / ", startIndex)) >= 0)
            {
                yield return value.Substring(startIndex, index - startIndex);
                startIndex = index + " / ".Length;
            }

            yield return value.Substring(startIndex);
        }

        private static string[][] GetModelMatrix(string[] models)
        {
            if (models[0].Contains('/'))
            {
                var models2 = models
                    .Select(Split)
                    .ToArray();
                return models.Length > 1
                    ? Transpose(models2)
                    : models2;
            }
            return new[] { models };
        }

        private static string[] Split(string model)
        {
            var split = model.Split(' ');
            var index = GetIndex(split);
            var submodels = split[index].Split('/');
            var result = new string[submodels.Length];
            for (int i = 0; i < submodels.Length; i++)
            {
                var split2 = new string[split.Length];
                Array.Copy(split, split2, split.Length);
                split2[index] = submodels[i];
                result[i] = string.Join(" ", split2);
            }
            return result;
        }

        private static string[][] Transpose(string[][] m)
        {
            var t = new string[m[0].Length][];
            for (int i = 0; i < m[0].Length; i++)
            {
                t[i] = new string[m.Length];
                for (int j = 0; j < m.Length; j++)
                    t[i][j] = m[j][i];
            }

            return t;
        }

        private static int GetIndex(string[] split)
        {
            for (int i = 0; i < split.Length; i++)
                if (split[i].Contains('/'))
                    return i;
            return -1;
        }
    }
}
