using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Net.Chdk.Meta.Generators.Platform.Ps
{
    sealed class PsPlatformGenerator : PsPlatformGeneratorBase
    {
        protected override string Keyword => "PowerShot";

        protected override string[] Suffixes => new[] { "IS" };

        private IIxusPlatformGenerator IxusGenerator { get; }

        public PsPlatformGenerator(IIxusPlatformGenerator ixusGenerator)
        {
            IxusGenerator = ixusGenerator;
        }

        public override string GetPlatform(uint modelId, string[] models)
        {
            var ps = base.GetPlatform(modelId, models);
            if (ps != null && models.Length > 1)
            {
                var ixus = IxusGenerator.Generate(modelId, models[1]);
                if (ixus != null)
                {
                    return $"{ixus}_{ps}";
                }
            }
            return ps;
        }

        protected override IEnumerable<string> PreGenerate(uint modelId, string source)
        {
            var split = base.PreGenerate(modelId, source);
            if (split == null)
                return null;

            return AdaptMark(split.ToArray());
        }

        protected override IEnumerable<string> Trim(IEnumerable<string> split)
        {
            Debug.Assert(split.Last().Equals("IS"));

            var split2 = split.Take(split.Count() - 1);
            var beforeLast = split2.Last();
            var index = GetIndexOfDigit(beforeLast);
            var series = beforeLast.Substring(0, index);
            var modelStr = beforeLast.Substring(index);
            uint model = uint.Parse(modelStr);
            switch (series)
            {
                case "": // ELPH
                case "A":
                case "SD":
                case "SX" when model < 100:
                    return split2;
                default:
                    return split;
            }
        }

        protected override IEnumerable<string> Process(IEnumerable<string> split)
        {
            if (split.Count() >= 2 && split.Skip(1).First().StartsWith("Facebook"))
                return new[] { "N_Facebook" };
            return split;
        }

        private static int GetIndexOfDigit(string value)
        {
            for (int i = 0; i < value.Length; i++)
                if (char.IsDigit(value[i]))
                    return i;
            return -1;
        }
    }
}
