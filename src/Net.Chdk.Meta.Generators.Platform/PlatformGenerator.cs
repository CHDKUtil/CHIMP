using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Generators.Platform
{
    sealed class PlatformGenerator : IPlatformGenerator
    {
        private IEnumerable<IInnerPlatformGenerator> Generators { get; }

        public PlatformGenerator(IEnumerable<IInnerPlatformGenerator> generators)
        {
            Generators = generators;
        }

        public string GetPlatform(uint modelId, string[] models)
        {
            if (models == null)
                throw new ArgumentNullException(nameof(models));

            if (models.Length == 0)
                throw new ArgumentException("At least one model name is required", nameof(models));

            if (models.Any(string.IsNullOrEmpty))
                throw new ArgumentException("Model names cannot be null or empty", nameof(models));

            return Generators
                .Select(g => g.GetPlatform(modelId, models))
                .FirstOrDefault(r => r != null);
        }
    }
}
