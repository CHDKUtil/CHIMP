using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Chimp
{
    internal static class LoggerExtensions
    {
        public static void LogObject(this ILogger logger, LogLevel level, object? obj)
        {
            logger.Log(level, default, obj, null, GetValue);
        }

        public static void LogObject(this ILogger logger, LogLevel level, string format, object? obj)
        {
            logger.Log(level, default, obj, null, (v, e) => GetValue(format, v));
        }

        private static string GetValue(string format, object? obj)
        {
            var value = GetValue(obj, null);
            return string.Format(format, value);
        }

        private static string GetValue(object? obj, Exception? ex)
        {
            var settings = new JsonSerializerSettings
            {
                Converters = new[] { new VersionConverter() }
            };
            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}
