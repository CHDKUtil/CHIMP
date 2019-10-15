using Chimp.Model;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using System;
using System.Collections.Generic;

namespace Chimp.Resolvers
{
    sealed class DownloadProviderResolver : ProviderResolver<IDownloadProvider, DownloadProvider>
    {
        public DownloadProviderResolver(IServiceActivator serviceActivator, IDictionary<string, Distro> distros)
            : base(serviceActivator, distros)
        {
        }

        protected override string GetTypeName(Distro distro)
        {
            return distro.ProductType;
        }

        protected override IEnumerable<Type> GetTypes()
        {
            yield return typeof(Uri);
        }

        protected override IEnumerable<object> GetValues(string sourceName, SoftwareSourceInfo source, Distro distro)
        {
            yield return distro.BaseUrl;
        }
    }
}
