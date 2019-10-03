using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Chdk.Generators.Platform
{
    abstract class InnerPlatformGenerator : IInnerPlatformGenerator
    {
        private const uint MinPsModelId = 0x1540000;

        public string? GetPlatform(uint modelId, IEnumerable<string[]> splits, string? category)
        {
            string? value = GetCategoryName(modelId, splits);
            if (category?.Equals(value) == false)
                return null;

            var split = PreGenerate(modelId, splits);
            if (split == null)
                return null;

            var split2 = Process(split);

            return PostProcess(split2);
        }

        protected virtual string? GetCategoryName(uint modelId, IEnumerable<string[]> splits)
        {
            return modelId < MinPsModelId
                ? string.Empty
                : "PS";
        }

        protected abstract string[]? PreGenerate(uint modelId, IEnumerable<string[]> splits);

        protected abstract IEnumerable<string> Process(string[] split);

        protected abstract string Keyword { get; }

        private static string PostProcess(IEnumerable<string> split)
        {
            return string.Join(string.Empty, split)
                .ToLowerInvariant();
        }

        protected static IEnumerable<string> AdaptMark(string[] split)
        {
            var index = Array.IndexOf(split, "Mark");
            if (index <= 0)
                return split;

            var m = RomanToInteger(split[index + 1]).ToString();
            if (char.IsDigit(split[index - 1][split[index - 1].Length - 1]))
                m = '_' + m;

            return split
                .Take(index)
                .Concat(new[] { m });
        }

        private static uint RomanToInteger(string roman)
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
