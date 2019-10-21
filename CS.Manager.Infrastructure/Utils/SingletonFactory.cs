using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Manager.Infrastructure.Utils
{
    public class SingletonFactory
    {

        Dictionary<Type, Dictionary<string, object>> serviceDict;
        public SingletonFactory()
        {
            serviceDict = new Dictionary<Type, Dictionary<string, object>>();
        }

        public TService GetService<TService>(string id) where TService : class
        {
            var serviceType = typeof(TService);
            return GetService<TService>(serviceType, id);
        }

        public TService GetService<TService>(Type serviceType, string id) where TService : class
        {
            if (serviceDict.TryGetValue(serviceType, out Dictionary<string, object> implDict))
            {
                if (implDict.TryGetValue(id, out object service))
                {
                    return service as TService;
                }
            }
            return null;
        }

        public void AddService<TService>(TService service, string id) where TService : class
        {
            AddService(typeof(TService), service, id);
        }

        public void AddService(Type serviceType, object service, string id)
        {
            if (service != null)
            {
                if (serviceDict.TryGetValue(serviceType, out Dictionary<string, object> implDict))
                {
                    implDict[id] = service;
                }
                else
                {
                    implDict = new Dictionary<string, object>();
                    implDict[id] = service;
                    serviceDict[serviceType] = implDict;
                }
            }
        }
    }
}
