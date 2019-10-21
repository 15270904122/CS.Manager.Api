using CS.Manager.EasyNetQ.Consumer;
using EasyNetQ;
using EasyNetQ.DI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS.Manager.EasyNetQ.Configuration
{
    public static class RabbitMqExtension
    {
        public static IApplicationBuilder UseRabbitMQSubscribe(this IApplicationBuilder appBuilder)
        {
            var services = appBuilder.ApplicationServices.CreateScope().ServiceProvider;
            services.GetServices<IBaseConsumer>().ToList().ForEach(x => x.InitSubscribe());
            return appBuilder;
        }
    }
}
