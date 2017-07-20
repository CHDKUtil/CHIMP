using System;

namespace Chimp
{
    public interface IServiceActivator
    {
        T Create<T>(string assemblyName, string typeName, Type[] argTypes, object[] argValues);
        T Create<T>(Type[] types, object[] values);
        T Create<T>();
    }
}
