using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomDIContainer
{
    public class DIContainer
    {
        private readonly Dictionary<Type, ServiceDescriptor> _services = new();
        private readonly Dictionary<Type, object> _singletonCache = new();

        // Register services
        public void Register<TService, TImplementation>(ServiceLifetime lifetime)
            where TImplementation : TService
        {
            var descriptor = new ServiceDescriptor(
                typeof(TService),
                typeof(TImplementation),
                lifetime);

            _services[typeof(TService)] = descriptor;
        }

        // Resolve generic
        public TService Resolve<TService>()
        {
            return (TService)Resolve(typeof(TService));
        }

        // Core resolve logic
        private object Resolve(Type serviceType)
        {
            if (!_services.TryGetValue(serviceType, out var descriptor))
                throw new InvalidOperationException($"Service '{serviceType.Name}' not registered");

            // Singleton handling
            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                if (!_singletonCache.TryGetValue(serviceType, out var instance))
                {
                    instance = CreateInstance(descriptor.ImplementationType);
                    _singletonCache[serviceType] = instance;
                }
                return instance;
            }

            // Transient
            return CreateInstance(descriptor.ImplementationType);
        }

        private object CreateInstance(Type implementationType)
        {
            // Constructor selection (greedy)
            var constructor = implementationType
                .GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault()
                ?? throw new InvalidOperationException(
                    $"No public constructor found on '{implementationType.Name}'.");

            // Resolve parameters recursively
            var parameters = constructor
                .GetParameters()
                .Select(p => Resolve(p.ParameterType))
                .ToArray();

            return constructor.Invoke(parameters);
        }
    }
}