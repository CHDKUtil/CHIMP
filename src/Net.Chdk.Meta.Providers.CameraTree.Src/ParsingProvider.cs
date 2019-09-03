using Microsoft.Extensions.Logging;
using System.IO;

namespace Net.Chdk.Meta.Providers.CameraTree.Src
{
    abstract class ParsingProvider<T> : ValueProvider
        where T : class
    {
        protected ParsingProvider(ILogger logger)
            : base(logger)
        {
        }

        protected T GetValue(string platformPath, string platform, string revision)
        {
            T value = null;

            var filePath = GetFilePath(platformPath, platform, revision);
            using (var reader = File.OpenText(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    if (line.StartsWith(Prefix))
                    {
                        line = TrimComments(line, platform, revision);
                        line = line.Substring(Prefix.Length).TrimStart();
                        UpdateValue(ref value, line, platform);
                    }
                }
            }

            return value;
        }

        protected abstract string Prefix { get; }

        protected abstract void UpdateValue(ref T value, string line, string platform);

        protected abstract string TrimComments(string line, string platform, string revision);
    }
}
