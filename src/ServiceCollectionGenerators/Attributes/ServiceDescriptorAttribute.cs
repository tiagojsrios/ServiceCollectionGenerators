using System;

namespace ServiceCollectionGenerators.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    internal class ServiceDescriptorAttribute : Attribute
    {
        public ServiceLifetimeEnum Lifetime { get; }

        public ServiceDescriptorAttribute(ServiceLifetimeEnum lifetime)
        {
            Lifetime = lifetime;
        }
    }

    internal enum ServiceLifetimeEnum
    {
        Scoped,
        Singleton,
        Transient
    }
}