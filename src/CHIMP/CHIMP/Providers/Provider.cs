using System;
using System.Collections.Generic;
using System.Reflection;

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

        protected TValue CreateProvider(string product, string assembly, string type, Type[] argTypes = null, object[] argValues = null)
        {
            var @namespace = GetNamespace(product);
            assembly ??= Assembly.GetExecutingAssembly().FullName;
            return ServiceActivator.Create<TValue>(assembly, $"{@namespace}.{type}{GetTypeSuffix()}", argTypes, argValues);
        }

        protected virtual string GetNamespace(string product) => throw new NotImplementedException();

        protected virtual string GetTypeSuffix() => throw new NotImplementedException();
    }
}
