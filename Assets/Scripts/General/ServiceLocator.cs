using System.Collections.Generic;
using UnityEngine;
using System;


namespace Dorkbots.ServiceLocatorTools {
    /// <summary>
    /// This class is used in the Service Locator Pattern (read more -> https://en.wikipedia.org/wiki/Service_locator_pattern).
    /// The service locator pattern is a design pattern used in software development to encapsulate the processes involved in obtaining a service with a strong abstraction layer.
    /// This pattern uses a central registry known as the “service locator” which on request returns the information necessary to perform a certain task.
    /// </summary>
    public static class ServiceLocator 
    {
        private static readonly Dictionary<Type, object> Services = new Dictionary<Type, object>();

        /// <summary>
        /// You need to register the service before it can be Resolved or accessed. You can only register one instance of one type.
        /// </summary>
        /// <param name="serviceInstance">An instance of the service</param>
        /// <typeparam name="T">The service type</typeparam>
        public static T Register<T>(object serviceInstance)
        {
            if (Services.ContainsKey(typeof(T)))
            {
                Debug.LogError("Service is already registered! Please unregister the current service before registering a new instance.");
            }
            else if (serviceInstance is T)
            {
                Services[typeof(T)] = serviceInstance;
            }
            else
            {
                Debug.LogError("Object does not derived from the service type!");
            }

            return (T)serviceInstance;
        }

        /// <summary>
        /// Unregisters current instance of a service.
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        public static void Unregister<T>()
        {
            if (Services.ContainsKey(typeof(T)))
            {
                Services.Remove(typeof(T));
            }
        }

        /// <summary>
        /// Returns the service. This will fail if the service was not first registered via the `Register` method.
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>Returns a registered instance of the type requested.</returns>
        public static T Resolve<T>()
        {
            return (T)Services[typeof(T)];
        }
        
        /// <summary>
        /// Checks if a service is registered.
        /// </summary>
        /// <typeparam name="T">The service type</typeparam>
        /// <returns>returns true if the service is registered.</returns>
        public static bool IsRegistered<T>()
        {
            return Services.ContainsKey(typeof(T));
        }
        
        /// <summary>
        /// Clears all registered services. BUT does not perform any dispose or cleanup on the services.
        /// </summary>
        public static void Reset()
        {
            Services.Clear();
        }
    }
}

