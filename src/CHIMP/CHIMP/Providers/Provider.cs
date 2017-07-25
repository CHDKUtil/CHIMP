using System;
using System.Collections.Generic;

namespace Chimp.Providers
{
    abstract class Provider<TData, TValue>
        where TData : class
    {
        protected IServiceActivator ServiceActivator { get; }

        protected Provider(IServiceActivator serviceActivator)
        {
            ServiceActivator = serviceActivator;
        }

        protected abstract IDictionary<string, TData> Data { get; }

        protected TValue CreateProvider(string key, string assembly, string type, Type[] argTypes = null, object[] argValues = null)
        {
            var @namespace = GetNamespace(key);
            return ServiceActivator.Create<TValue>(assembly, $"{@namespace}.{type}{TypeSuffix}", argTypes, argValues);
        }

        protected virtual string Namespace => throw new NotImplementedException();

        protected virtual string TypeSuffix => throw new NotImplementedException();

        protected virtual string GetNamespace(string key) => Namespace;
    }
}
