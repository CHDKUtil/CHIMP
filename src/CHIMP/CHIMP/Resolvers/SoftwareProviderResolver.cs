using Chimp.Model;
using Net.Chdk.Model.Software;
using Net.Chdk.Providers.Software;
using Net.Chdk.Providers.Software.Script;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Chimp.Resolvers
{
    sealed class SoftwareProviderResolver : ProviderResolver<ISoftwareProvider, SoftwareProvider, ScriptSoftwareProvider>
    {
        public SoftwareProviderResolver(IServiceActivator serviceActivator, IDictionary<string, Distro> distros)
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
            yield return typeof(CultureInfo);
        }

        protected override IEnumerable<object> GetValues(string sourceName, SoftwareSourceInfo source, Distro distro)
        {
            yield return source;
            yield return distro.Language;
        }
    }
}
