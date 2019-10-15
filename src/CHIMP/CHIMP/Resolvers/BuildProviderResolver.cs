using Chimp.Model;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using System;
using System.Collections.Generic;

namespace Chimp.Resolvers
{
    sealed class BuildProviderResolver : ProviderResolver<IBuildProvider, BuildProvider>
    {
        public BuildProviderResolver(IServiceActivator serviceActivator, IDictionary<string, Distro> distros)
            : base(serviceActivator, distros)
        {
        }

        public Type GetProviderType(Distro distro)
        {
            return typeof(IBuildProvider);
        }

        protected override string GetTypeName(Distro distro)
        {
            return distro.ProductType;
        }

        protected override IEnumerable<Type> GetTypes(Distro distro)
        {
            yield return typeof(SoftwareSourceInfo);
        }

        protected override IEnumerable<object> GetValues(string sourceName, SoftwareSourceInfo source, Distro distro)
        {
            yield return source;
        }
    }
}
