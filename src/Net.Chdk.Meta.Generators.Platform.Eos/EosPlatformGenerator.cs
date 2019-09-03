using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Generators.Platform.Eos
{
    sealed class EosPlatformGenerator : InnerPlatformGenerator
    {
        public override string GetPlatform(string[] models)
        {
            if (models[0].Contains("Rebel"))
            {
                if (models.Length > 1)
                {
                    var model = models[1];
                    if (!model.StartsWith("EOS "))
                        model = $"EOS {model}";
                    return base.GetPlatform(new[] { model });
                }
                return null;
            }
            return base.GetPlatform(models);
        }

        protected override IEnumerable<string> PreGenerate(string source)
        {
            var split = source.Split(' ');
            if (!Keyword.Equals(split[0]))
                return null;

            if (split[1].Equals("M"))
                return new[] { "EOSM" };

            if (split[1].StartsWith("M"))
                return null;

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
