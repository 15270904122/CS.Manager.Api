using CS.Manager.Application.RabbitMq.Interfaces;
using CS.Manager.EasyNetQ.ConsumeModels;
using EasyNetQ;
using EasyNetQ.Consumer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CS.Manager.Application.RabbitMq
{
    public class RabbitMqAppService : IRabbitMqAppService
    {
        private IBus _bus;
        private IServiceProvider _serviceProvider;
        public RabbitMqAppService(IBus bus, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <returns></returns>
        public async Task Pulish()
        {
            await _bus.PublishAsync(new Test { TaskId = Guid.NewGuid() });
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <returns></returns>
        public async Task PulishError()
        {
            await _bus.PublishAsync(new Test { TaskId = Guid.Empty });
        }
    }
}
