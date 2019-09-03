using Net.Chdk.Meta.Generators.Platform;
using Net.Chdk.Meta.Model.Platform;
using System;
using System.Collections.Generic;
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

        public IDictionary<string, PlatformData> GetPlatforms(string path)
        {
            var provider = GetInnerProvider(path, out string ext);
            if (provider == null)
                throw new InvalidOperationException($"Unknown platform extension: {ext}");
            using (var reader = File.OpenText(path))
            {
                var keys = provider.GetPlatforms(reader);
                return GetPlatforms(keys);
            }
        }

        private IDictionary<string, PlatformData> GetPlatforms(IEnumerable<KeyValuePair<string, string>> values)
        {
            var platforms = new SortedDictionary<string, PlatformData>();
            foreach (var kvp in values)
            {
                var models = GetCameraModels(kvp.Value);
                if (models.First() != null)
                {
                    foreach (var model in models)
                    {
                        var platform = GetPlatform(kvp.Key, model);
                        platforms.Add(model.Platform, platform);
                    }
                }
            }
            return platforms;
        }

        private static PlatformData GetPlatform(string modelId, CameraModel model)
        {
            return new PlatformData
            {
                ModelId = modelId,
                Names = model.Names
            };
        }

        private IEnumerable<CameraModel> GetCameraModels(string value)
        {
            var models = GetModels(value)
                .Select(m => m.TrimEnd(" (new)"))
                .ToArray();

            return GetModelMatrix(models)
                .Select(GetCameraModel);
        }

        private CameraModel GetCameraModel(string[] names)
        {
            var platform = GetPlatform(names);
            if (platform == null)
                return null;

            return new CameraModel
            {
                Names = GetNames(names).ToArray(),
                Platform = platform
            };
        }

        private string GetPlatform(string[] names)
        {
            return PlatformGenerator.GetPlatform(names);
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
