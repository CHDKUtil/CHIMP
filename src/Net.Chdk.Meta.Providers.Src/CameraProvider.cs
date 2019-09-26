using Microsoft.Extensions.Logging;
using System;

namespace Net.Chdk.Meta.Providers.Src
{
    public abstract class CameraProvider<TCamera> : HeaderParsingProvider<TCamera>
        where TCamera : class
    {
        protected CameraProvider(ILogger logger)
            : base(logger)
        {
        }

        public TCamera? GetCamera(string platformPath, string platform)
        {
            return GetValue(platformPath, platform, null);
        }

        protected override string FileName => "platform_camera.h";

        protected bool GetBoolean(string[] split, string platform)
        {
            var value = split[split.Length - 1];
            if (!"1".Equals(value))
            {
                var name = GetName(platform);
                throw new InvalidOperationException($"{name}: Unexpected value {value}");
            }
            return true;
        }
    }
}
