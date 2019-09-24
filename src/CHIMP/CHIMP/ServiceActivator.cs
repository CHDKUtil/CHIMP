using System;
using System.Linq;

namespace Chimp
{
    public sealed class ServiceActivator : IServiceActivator
    {
        private IServiceProvider ServiceProvider { get; }

        public ServiceActivator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public T Create<T>(string typeName, Type[]? argTypes, object[]? argValues)
        {
            var type = Type.GetType(typeName);
            if (type == null)
                throw new TypeInitializationException(typeName, new TypeLoadException());
            return Create<T>(type, typeName, argTypes, argValues);
        }

        public T Create<T>()
        {
            return Create<T>(new Type[0], new object[0]);
        }

        public T Create<T>(Type[] argTypes, object[] argValues)
        {
            return Create<T>(typeof(T), typeof(T).Name, argTypes, argValues);
        }

        private T Create<T>(Type type, string typeName, Type[]? argTypes, object[]? argValues)
        {
            try
            {
                var ctors = type.GetConstructors();
                var ctor = ctors.SingleOrDefault();
                var parms = ctor.GetParameters();
                var args = new object?[parms.Length];
                for (int i = 0; i < parms.Length; i++)
                    args[i] = GetArgument(parms[i].ParameterType, argTypes, argValues);
                return (T)ctor.Invoke(args);
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(typeName, ex);
            }
        }

        private object? GetArgument(Type type, Type[]? argTypes, object[]? argValues)
        {
            if (argTypes != null)
            {
                var index = Array.IndexOf(argTypes, type);
                if (index >= 0)
                    return argValues?[index];
            }
            return ServiceProvider.GetService(type)
                ?? throw new TypeLoadException($"Cannot resolve type {type}");
        }
    }
}
