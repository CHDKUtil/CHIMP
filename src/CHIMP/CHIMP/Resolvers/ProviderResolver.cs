using Chimp.Model;
using Chimp.Providers;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Resolvers
{
    abstract class ProviderResolver<TProvider> : DataProvider<Distro, TProvider>
        where TProvider : class
    {
        private ConcurrentDictionary<string, TProvider> Providers { get; }
        private IDictionary<string, Distro> Distros { get; }

        protected ProviderResolver(IServiceActivator serviceActivator, IDictionary<string, Distro> distros)
            : base(serviceActivator)
        {
            Providers = new ConcurrentDictionary<string, TProvider>();
            Distros = distros;
        }

        public TProvider GetProvider(string sourceName, SoftwareSourceInfo source)
        {
            return Providers.GetOrAdd(sourceName, _ => CreateProvider(sourceName, source));
        }

        protected override IDictionary<string, Distro> Data => Distros ?? base.Data;

        private TProvider CreateProvider(string sourceName, SoftwareSourceInfo source)
        {
            if (!Data.TryGetValue(sourceName, out Distro distro))
                return null;

            var assemblyName = GetAssemblyName(distro);
            var typeName = GetTypeName(distro);
            var types = GetTypes(distro).ToArray();
            var values = GetValues(sourceName, source, distro).ToArray();

            return CreateProvider(distro.ProductType, assemblyName, typeName, types, values);
        }

        protected virtual string GetAssemblyName(Distro distro)
        {
            return distro.Assembly;
        }

        protected abstract string GetTypeName(Distro distro);
        protected abstract IEnumerable<object> GetValues(string sourceName, SoftwareSourceInfo source, Distro distro);
        protected abstract IEnumerable<Type> GetTypes(Distro distro);

        protected override string GetFilePath()
        {
            throw new NotImplementedException();
        }

        protected override string GetNamespace(string product)
        {
            return $"{typeof(TProvider).Namespace}.{product}";
        }

        protected sealed override string GetTypeSuffix()
        {
            return typeof(TProvider).Name.Substring(1);
        }
    }

    abstract class ProviderResolver<TProvider, TProvider1, TProvider2> : ProviderResolver<TProvider>
        where TProvider : class
        where TProvider1 : TProvider
        where TProvider2 : TProvider
    {
        protected ProviderResolver(IServiceActivator serviceActivator, IDictionary<string, Distro> distros)
            : base(serviceActivator, distros)
        {
        }

        public Type GetProviderType(Distro distro)
        {
            return distro.MatchType != "Script"
                ? typeof(TProvider1)
                : typeof(TProvider2);
        }
    }
}
