using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CS.Manager.Application.RabbitMq.Interfaces
{
    /// <summary>
    /// RabbitMq应用服务
    /// </summary>
    public interface IRabbitMqAppService
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <returns></returns>
        Task Pulish();

        /// <summary>
        /// 发布异常消息
        /// </summary>
        /// <returns></returns>
        Task PulishError();
        
    }
}
