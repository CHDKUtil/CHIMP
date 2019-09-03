using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Meta.Generators.Platform
{
    public abstract class InnerPlatformGenerator : IInnerPlatformGenerator
    {
        public virtual string GetPlatform(string[] models)
        {
            return Generate(models[0]);
        }

        public string Generate(string source)
        {
            var split = PreGenerate(source);
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

        protected static string[] AdaptMark(string[] split)
        {
            var index = Array.IndexOf(split, "Mark");
            if (index <= 0)
                return split;
            var m = RomanToInteger(split[index + 1]);
            return split
                .Take(index)
                .Concat(new[] { m.ToString() })
                .ToArray();
        }

        protected virtual IEnumerable<string> PreGenerate(string source)
        {
            var split = source.Split(' ');
            if (!Keyword.Equals(split[0]))
                return null;
            return split.Skip(1);
        }

        protected virtual IEnumerable<string> Process(IEnumerable<string> split)
        {
            return split;
        }

        protected virtual string PostProcess(IEnumerable<string> split)
        {
            return string.Join(string.Empty, split)
                .ToLower();
        }

        protected virtual IEnumerable<string> Trim(IEnumerable<string> split)
        {
            return split.Take(split.Count() - 1);
        }

        protected abstract string Keyword { get; }

        protected virtual string[] Suffixes => new string[0];

        private static int RomanToInteger(string roman)
        {
            switch (roman)
            {
                case "I":
                    return 1;
                case "II":
                    return 2;
                case "III":
                    return 3;
                case "IV":
                    return 4;
                default:
                    throw new InvalidOperationException($"Invalid numeral {roman}");
            }
        }
    }
}
