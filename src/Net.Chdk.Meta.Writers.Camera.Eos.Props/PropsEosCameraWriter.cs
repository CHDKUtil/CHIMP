using Net.Chdk.Meta.Model.Camera.Eos;
using Net.Chdk.Meta.Writers.Camera.Props;
using System;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Writers.Camera.Eos.Props
{
    sealed class PropsEosCameraWriter : PropsCameraWriter<EosCameraData, EosCameraModelData, EosCardData>, IEosInnerCameraWriter
    {
        protected override void WriteModel(TextWriter writer, uint id, EosCameraModelData model)
        {
            WriteVersions(writer, id, model.Platform, model.Versions);
        }

        private static void WriteVersions(TextWriter writer, uint id, string platform, IDictionary<string, VersionData> versions)
        {
            foreach (var kvp in versions)
            {
                WriteVersion(writer, id, platform, kvp.Key, kvp.Value.Version);
            }
        }

        private static void WriteVersion(TextWriter writer, uint id, string platform, string versionKey, string versionValue)
        {
            var version = GetVersion(versionKey);
            writer.WriteLine($"0x{id:x}-{version}={platform}-{versionValue}");
        }

        private static string GetVersion(string versionKey)
        {
            var version = Version.Parse(versionKey);
            return $"{version.Major}{version.Minor}{version.Build}";
        }
    }
}
