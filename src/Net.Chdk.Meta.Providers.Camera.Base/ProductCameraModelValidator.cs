using Microsoft.Extensions.Logging;
using Net.Chdk.Meta.Model.CameraList;
using Net.Chdk.Meta.Model.CameraTree;
using System;
using System.Collections.Generic;

namespace Net.Chdk.Meta.Providers.Camera
{
    public abstract class ProductCameraModelValidator : IProductCameraModelValidator
    {
        protected ILogger Logger { get; }

        protected ProductCameraModelValidator(ILogger logger)
        {
            Logger = logger;
        }

        public abstract string ProductName { get; }

        public void Validate(string platform, ListPlatformData list, TreePlatformData tree)
        {
            if (list?.Revisions == null)
                throw new InvalidOperationException($"{platform}: null list/revisions");

            if (tree?.Revisions == null)
                throw new InvalidOperationException($"{platform}: null tree/revisions");

            foreach (var kvp in tree.Revisions)
                Validate(kvp, platform, list.Revisions);

            foreach (var kvp in list.Revisions)
                Validate(kvp, platform, tree.Revisions);
        }

        private void Validate(KeyValuePair<string, TreeRevisionData> kvp, string platform, IDictionary<string, ListRevisionData> listRevisions)
        {
            var revision = kvp.Key;
            if (!listRevisions.ContainsKey(revision))
                OnListRevisionMissing(platform, revision);
        }

        private void Validate(KeyValuePair<string, ListRevisionData> kvp, string platform, IDictionary<string, TreeRevisionData> treeRevisions)
        {
            var revision = kvp.Key;
            if (!treeRevisions.ContainsKey(revision))
                OnTreeRevisionMissing(platform, revision);
            var sourceRevision = kvp.Value?.Source?.Revision;
            if (sourceRevision != null && !treeRevisions.ContainsKey(sourceRevision))
                OnTreeRevisionMissing(platform, revision, sourceRevision);
        }

        protected virtual void OnListRevisionMissing(string platform, string revision)
        {
            throw new InvalidOperationException($"{platform}: {revision} missing from list");
        }

        protected virtual void OnTreeRevisionMissing(string platform, string revision)
        {
            throw new InvalidOperationException($"{platform}: {revision} missing from tree");
        }

        protected virtual void OnTreeRevisionMissing(string platform, string revision, string sourceRevision)
        {
            throw new InvalidOperationException($"{platform}-{revision}: {sourceRevision} missing from tree");
        }
    }
}
