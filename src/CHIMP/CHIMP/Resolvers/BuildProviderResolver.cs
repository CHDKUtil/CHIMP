using Chimp.Model;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using Net.Chdk.Providers.Software.Script;
using System;
using System.Collections.Generic;

namespace Chimp.Resolvers
{
    sealed class BuildProviderResolver : ProviderResolver<IBuildProvider, BuildProvider, ScriptBuildProvider>
    {
        public BuildProviderResolver(IServiceActivator serviceActivator, IDictionary<string, Distro> distros)
            : base(serviceActivator, distros)
        {
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
