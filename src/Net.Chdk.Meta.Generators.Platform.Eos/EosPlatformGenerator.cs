using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Generators.Platform.Eos
{
    sealed class EosPlatformGenerator : InnerPlatformGenerator
    {
        private static readonly string[] PsEosModels = new[] { "M3", "M5", "M6", "M10", "M100" };

        public override string GetPlatform(uint modelId, string[] models)
        {
            if (models[0].Contains("Rebel"))
            {
                if (models.Length < 2)
                    return null;
                var model = models[1];
                if (!model.StartsWith("EOS "))
                    model = $"EOS {model}";
                models = new[] { model };
            }
            return base.GetPlatform(modelId, models);
        }

        protected override IEnumerable<string> PreGenerate(uint modelId, string source)
        {
            var split = source.Split(' ');
            if (!Keyword.Equals(split[0]))
                return null;

            var model = split[1];
            if (model.StartsWith("M"))
            {
                if (PsEosModels.Contains(model) && split.Length == 2)
                    return null;
                split[1] = $"EOS{model}";
            }

            split = AdaptMark(split);

            if (split[split.Length - 1].Equals("5D"))
                return new[] { "5DC" };

            return split.Skip(1);
        }

        protected override IEnumerable<string> Process(IEnumerable<string> split)
        {
            return split;
        }

        protected override string PostProcess(IEnumerable<string> split)
        {
            return string.Join(string.Empty, split);
        }

        protected override string Keyword => "EOS";
    }
}
