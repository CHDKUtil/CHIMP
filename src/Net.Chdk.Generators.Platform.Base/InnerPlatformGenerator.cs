using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Generators.Platform
{
    public abstract class InnerPlatformGenerator : IInnerPlatformGenerator
    {
        public virtual string? GetPlatform(uint modelId, string[] models)
        {
            return Generate(modelId, models[0]);
        }

        public string? Generate(uint modelId, string source)
        {
            var split = PreGenerate(modelId, source);
            if (split == null)
                return null;

            split = Process(split);
            if (split == null)
                return null;

            var last = split.Last();
            if (Suffixes.Any(t => t.Equals(last)) == true)
                split = Trim(split);

            return PostProcess(split);
        }

        public abstract string CategoryName { get; }

        protected static string[] AdaptMark(string[] split)
        {
            var index = Array.IndexOf(split, "Mark");
            if (index <= 0)
                return split;

            var m = RomanToInteger(split[index + 1]);
            split = split
                .Take(index)
                .Concat(new[] { m.ToString() })
                .ToArray();

            if (char.IsDigit(split[index - 1][split[index - 1].Length - 1]))
                split[index] = '_' + split[index];

            return split;
        }

        protected virtual IEnumerable<string>? Process(IEnumerable<string> split)
        {
            return split;
        }

        protected virtual string PostProcess(IEnumerable<string> split)
        {
            return string.Join(string.Empty, split);
        }

        protected virtual IEnumerable<string> Trim(IEnumerable<string> split)
        {
            return split.Take(split.Count() - 1);
        }

        protected abstract IEnumerable<string>? PreGenerate(uint modelId, string source);

        protected abstract string Keyword { get; }

        protected virtual string[] Suffixes => new string[0];

        private static int RomanToInteger(string roman)
        {
            return roman switch
            {
                "I" => 1,
                "II" => 2,
                "III" => 3,
                "IV" => 4,
                _ => throw new InvalidOperationException($"Invalid numeral {roman}"),
            };
        }
    }
}
