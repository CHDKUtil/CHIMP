using Net.Chdk.Meta.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Net.Chdk.Meta.Providers.Csv
{
    public abstract class CsvCameraProvider<TPlatform, TRevision, TSource>
        where TPlatform : PlatformData<TPlatform, TRevision, TSource>, new()
        where TRevision : RevisionData<TRevision, TSource>, new()
        where TSource : SourceData<TSource>, new()
    {
        public string Extension => ".csv";

        protected IDictionary<string, TPlatform> GetCameras(string path)
        {
            var cameras = new SortedDictionary<string, TPlatform>();
            using (var reader = File.OpenText(path))
            {
                reader.ReadLine();

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var split = line.Split(',');
                    if (split.Length != 5)
                        throw new InvalidOperationException("Invalid file format");
                    AddCamera(cameras, split);
                }
            }
            return cameras;
        }

        private void AddCamera(IDictionary<string, TPlatform> cameras, string[] split)
        {
            var platformData = GetOrAddPlatform(cameras, split[0]);
            var revisionData = GetRevisionData(split);
            platformData.Revisions.Add(split[1], revisionData);
        }

        private TPlatform GetOrAddPlatform(IDictionary<string, TPlatform> cameras, string platform)
        {
            TPlatform platformData;
            if (!cameras.TryGetValue(platform, out platformData))
            {
                platformData = GetPlatformData(platform);
                cameras.Add(platform, platformData);
            }
            return platformData;
        }

        protected virtual TPlatform GetPlatformData(string platform)
        {
            return new TPlatform
            {
                Revisions = new SortedDictionary<string, TRevision>()
            };
        }

        protected virtual TRevision GetRevisionData(string[] split)
        {
            return new TRevision
            {
                Source = GetSourceData(split)
            };
        }

        protected virtual TSource GetSourceData(string[] split)
        {
            return new TSource
            {
                Revision = GetRevision(split)
            };
        }

        private static string GetRevision(string[] split)
        {
            var revision = split[1];
            var source = split[3];
            if (!string.IsNullOrEmpty(source))
                return source;
            return revision;
        }
    }
}
