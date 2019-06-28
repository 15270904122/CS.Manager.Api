using EasyNetQ;
using EasyNetQ.ConnectionString;
using EasyNetQ.DI;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Manager.EasyNetQ.Configuration
{
    public static class EasyNetQServiceCollectionExtensions
    {
        public static IServiceCollection RegisterEasyNetQ(this IServiceCollection serviceCollection, Func<IServiceResolver, ConnectionConfiguration> connectionConfigurationFactory, Action<IServiceRegister> registerServices)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            var serviceRegister = new ServiceCollectionAdapter(serviceCollection);
            RabbitHutch.RegisterBus(serviceRegister, connectionConfigurationFactory, registerServices);
            return serviceCollection;
        }

        public static IServiceCollection RegisterEasyNetQ(this IServiceCollection serviceCollection, Func<IServiceResolver, ConnectionConfiguration> connectionConfigurationFactory)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            return serviceCollection.RegisterEasyNetQ(connectionConfigurationFactory, c => { });
        }

        public static IServiceCollection RegisterEasyNetQ(this IServiceCollection serviceCollection, string connectionString, Action<IServiceRegister> registerServices)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            return serviceCollection.RegisterEasyNetQ(c => c.Resolve<IConnectionStringParser>().Parse(connectionString), registerServices);
        }

        public static IServiceCollection RegisterEasyNetQ(this IServiceCollection serviceCollection, string connectionString)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            return serviceCollection.RegisterEasyNetQ(c => c.Resolve<IConnectionStringParser>().Parse(connectionString));
        }
    }
}
