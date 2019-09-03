using Net.Chdk.Meta.Model.Camera.Ps;
using Net.Chdk.Meta.Writers.Camera.Props;
using System;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Writers.Camera.Ps.Props
{
    sealed class PropsPsCameraWriter : PropsCameraWriter<PsCameraData, PsCameraModelData, PsCardData>, IPsInnerCameraWriter
    {
        protected override void WriteModel(TextWriter writer, uint id, PsCameraModelData model)
        {
            WriteRevisions(writer, id, model.Platform, model.Revisions);
        }

        private static void WriteRevisions(TextWriter writer, uint id, string platform, IDictionary<string, RevisionData> revisions)
        {
            foreach (var kvp in revisions)
            {
                WriteRevision(writer, id, platform, kvp.Key, kvp.Value.Revision);
            }
        }

        private static void WriteRevision(TextWriter writer, uint id, string platform, string revisionKey, string revisionValue)
        {
            var revision = GetRevision(revisionKey);
            writer.WriteLine($"0x{id:x}-{revision}={platform}-{revisionValue}");
        }

        private static string GetRevision(string revisionKey)
        {
            var revision = Convert.ToUInt32(revisionKey, 16);
            return new string(new[] {
                (char)(((revision >> 24) & 0x0f) + 0x30),
                (char)(((revision >> 20) & 0x0f) + 0x30),
                (char)(((revision >> 16) & 0x0f) + 0x30),
                (char)(((revision >>  8) & 0x7f) + 0x60)
            });
        }
    }
}
