using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CS.Manager.EasyNetQ.ConsumeModels;
using Microsoft.Extensions.Logging;

namespace CS.Manager.EasyNetQ.Consumer
{
    public class TestConsumer : IBaseConsumer
    {
        private readonly IBus _bus;
        private readonly ILogger _logger;
        public TestConsumer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            _bus = serviceProvider.GetService<IBus>();
            _logger = loggerFactory.CreateLogger(this.GetType().FullName);
        }
        public void InitSubscribe()
        {
            _bus.SubscribeAsync<Test>("CS_Manager_Test",
              async input => await ProcessMessage(input));
        }

        public async Task ProcessMessage(Test test)
        {
            _logger.LogInformation($"接收到了消息:{test.TaskId}");
            if (test.TaskId == Guid.Empty)
            {
                throw new Exception("异常了");
            }
        }
    }
}
