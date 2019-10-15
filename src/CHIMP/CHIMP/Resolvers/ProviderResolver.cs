﻿using Chimp.Model;
using Chimp.Providers;
using Net.Chdk.Model.Software;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Chimp.Resolvers
{
    abstract class ProviderResolver<TProvider, TProviderImpl> : DataProvider<Distro, TProvider>
        where TProviderImpl : TProvider
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
            var types = GetTypes().ToArray();
            var values = GetValues(sourceName, source, distro).ToArray();

            return CreateProvider(distro.ProductType, assemblyName, typeName, types, values);
        }

        protected virtual string GetAssemblyName(Distro distro)
        {
            return distro.Assembly;
        }

        protected abstract string GetTypeName(Distro distro);
        protected abstract IEnumerable<object> GetValues(string sourceName, SoftwareSourceInfo source, Distro distro);
        protected abstract IEnumerable<Type> GetTypes();

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
            return typeof(TProviderImpl).Name;
        }
    }
}
