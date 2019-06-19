using CS.Manager.EasyNetQ.Consumer;
using EasyNetQ;
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
        public static IApplicationBuilder UseRabbitMQ(this IApplicationBuilder appBuilder)
        {
            var services = appBuilder.ApplicationServices.CreateScope().ServiceProvider;

            var lifeTime = services.GetService<IApplicationLifetime>();
            var bus = services.GetService<IBus>();
            lifeTime.ApplicationStarted.Register(() =>
            {
                appBuilder.ApplicationServices.GetServices<IBaseConsumer>().ToList().ForEach(x => x.InitSubscribe());
            });
            lifeTime.ApplicationStopped.Register(() => { bus.Dispose(); });
            return appBuilder;
        }
    }
}
