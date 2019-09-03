using Net.Chdk.Meta.Providers.Platform.Exif;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Net.Chdk.Meta.Providers.Platform.Html
{
    sealed class HtmlPlatformProvider : ExifPlatformProvider
    {
        private static readonly Regex regex = new Regex("<tr><td class=r>(0x[0-9a-f]+)</td><td>= (.+)</td>$");

        protected override IEnumerable<KeyValuePair<string, string>> GetPlatforms(TextReader reader)
        {
            string line;

            while ((line = reader.ReadLine()) != null && line != "<tr class=h><th>Value</th><th>CanonModelID</th></tr>")
            {
            }

            while ((line = reader.ReadLine()) != null)
            {
                if (line.Length > 0)
                {
                    var match = regex.Match(line);
                    if (match.Success)
                    {
                        var modelId = match.Groups[1].Value;
                        var modelsStr = match.Groups[2].Value;
                        yield return new KeyValuePair<string, string>(modelId, modelsStr);
                    }
                }
            }
        }

        public override string Extension => ".html";
    }
}
