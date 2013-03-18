using System;
using Cdh.Toolkit.Extensions;
using System.Collections.Generic;

namespace Cdh.Toolkit.Services
{
    public interface IServiceManager
    {
        void RegisterService(IService service);
        void UnregisterService(IService service);

        IEnumerable<T> GetServices<T>() where T : IService;
        T GetService<T>() where T : IService;
    }

    public static class ServiceManagerExtensions
    {
        public static T RequireService<T>(this IServiceManager serviceManager) where T : IService
        {
            Check.ArgumentIsNotNull(serviceManager, "serviceManager");

            var service = serviceManager.GetService<T>();
            
            if (service == null) {
                throw new InvalidOperationException(string.Format(
                    "There is not exactly one service of type {0} registered.", typeof(T).FullName));
            }
            
            return service;
        }
        
        public static void RegisterAndStartService(this IServiceManager serviceManager, IService service)
        {
            Check.ArgumentIsNotNull(serviceManager, "serviceManager");
            Check.ArgumentIsNotNull(service, "service");

            serviceManager.RegisterService(service);
            service.Start();
        }
    }
}

