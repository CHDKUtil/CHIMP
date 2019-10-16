using Chimp.Downloaders;
using Chimp.Model;
using Net.Chdk;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Generic;
using System.IO;

namespace Chimp.Resolvers
{
    sealed class DownloaderResolver : ProviderResolver<IDownloader>, IDownloaderProvider
    {
        private string ProductName { get; }

        public DownloaderResolver(string productName, IServiceActivator serviceActivator)
            : base(serviceActivator, distros: null)
        {
            ProductName = productName;

            _build = new Lazy<BuildProviderResolver>(CreateBuild);
            _match = new Lazy<MatchProviderResolver>(CreateMatch);
            _software = new Lazy<SoftwareProviderResolver>(CreateSoftware);
            _download = new Lazy<DownloadProviderResolver>(CreateDownload);
        }

        public IDownloader GetDownloader(string productName, string sourceName, SoftwareSourceInfo source)
        {
            if (productName != null && !ProductName.Equals(productName))
                return null;
            return GetProvider(sourceName, source);
        }

        protected override string GetTypeName(Distro distro)
        {
            return distro.DownloadType;
        }

        protected override string GetAssemblyName(Distro distro)
        {
            return null;
        }

        protected override string GetNamespace(string product)
        {
            return typeof(Downloader).Namespace;
        }

        protected override IEnumerable<Type> GetTypes(Distro distro)
        {
            yield return Build.GetProviderType(distro);
            yield return Match.GetProviderType(distro);
            yield return Software.GetProviderType(distro);
            yield return Download.GetProviderType(distro);
        }

        protected override IEnumerable<object> GetValues(string sourceName, SoftwareSourceInfo source, Distro distro)
        {
            yield return Build.GetProvider(sourceName, source);
            yield return Match.GetProvider(sourceName, source);
            yield return Software.GetProvider(sourceName, source);
            yield return Download.GetProvider(sourceName, source);
        }

        #region FileName

        protected sealed override string GetFilePath()
        {
            return Path.Combine(Directories.Data, Directories.Product, ProductName, "distros.json");
        }

        #endregion

        #region Match

        private readonly Lazy<MatchProviderResolver> _match;

        private MatchProviderResolver Match => _match.Value;

        private MatchProviderResolver CreateMatch()
        {
            return new MatchProviderResolver(ServiceActivator, Data);
        }

        #endregion

        #region Product

        private readonly Lazy<SoftwareProviderResolver> _software;

        private SoftwareProviderResolver Software => _software.Value;

        private SoftwareProviderResolver CreateSoftware()
        {
            return new SoftwareProviderResolver(ServiceActivator, Data);
        }

        #endregion

        #region Build

        private readonly Lazy<BuildProviderResolver> _build;

        private BuildProviderResolver Build => _build.Value;

        private BuildProviderResolver CreateBuild()
        {
            return new BuildProviderResolver(ServiceActivator, Data);
        }

        #endregion

        #region Download

        private readonly Lazy<DownloadProviderResolver> _download;

        private DownloadProviderResolver Download => _download.Value;

        private DownloadProviderResolver CreateDownload()
        {
            return new DownloadProviderResolver(ServiceActivator, Data);
        }

        #endregion
    }
}
