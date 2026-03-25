using System;
using System.Collections.Generic;
using System.Text;

namespace CustomDIContainer
{
    using System.Reflection;

    public class DIContainer
    {
        private readonly List<ServiceDescriptor> _services = new();

        // Register services
        public void Register<TService, TImplementation>(ServiceLifetime lifetime)
        {
            _services.Add(new ServiceDescriptor(
                typeof(TService),
                typeof(TImplementation),
                lifetime));
        }

        // Resolve generic
        public TService Resolve<TService>()
        {
            return (TService)Resolve(typeof(TService));
        }

        // Core resolve logic
        private object Resolve(Type serviceType)
        {
            var descriptor = _services.FirstOrDefault(x => x.ServiceType == serviceType);

            if (descriptor == null)
                throw new Exception($"Service {serviceType.Name} not registered");

            // Singleton handling
            if (descriptor.Lifetime == ServiceLifetime.Singleton && descriptor.Instance != null)
            {
                return descriptor.Instance;
            }

            var implementationType = descriptor.ImplementationType;

            // Constructor injection
            var constructor = implementationType.GetConstructors().First();

            var parameters = constructor.GetParameters()
                .Select(p => Resolve(p.ParameterType))
                .ToArray();

            var instance = Activator.CreateInstance(implementationType, parameters);

            // Store singleton
            if (descriptor.Lifetime == ServiceLifetime.Singleton)
            {
                descriptor.Instance = instance;
            }

            return instance;
        }
    }
}
