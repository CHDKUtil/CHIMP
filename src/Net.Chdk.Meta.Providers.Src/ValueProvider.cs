using Microsoft.Extensions.Logging;
using System.IO;

namespace Net.Chdk.Meta.Providers.Src
{
    public abstract class ValueProvider
    {
        protected ILogger Logger { get; }

        protected ValueProvider(ILogger logger)
        {
            Logger = logger;
        }

        protected string GetFilePath(string basePath, string platform, string? revision)
        {
            var path = GetPath(basePath, platform, revision);
            return Path.Combine(path, FileName);
        }

        protected static string GetPath(string basePath, string platform, string? revision)
        {
            return revision != null
                ? Path.Combine(basePath, platform, "sub", revision)
                : Path.Combine(basePath, platform);
        }

        protected static string GetName(string platform, string? revision = null)
        {
            return revision != null
                ? $"{platform}-{revision}"
                : platform;
        }

        protected abstract string FileName { get; }
    }
}
