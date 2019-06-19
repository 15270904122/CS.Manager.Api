using CS.Manager.Application.Auth.Interfaces;
using CS.Manager.Application.RabbitMq.Interfaces;
using CS.Manager.Dto.Auth;
using CS.Manager.Infrastructure.Result;
using CS.Manager.Infrastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Manager.Api.Controllers
{
    /// <summary>
    /// 消息队列
    /// </summary>
    public class RabbitMqController : ApiController
    {
        public RabbitMqController()
        {
        }

        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="rabbitMqAppService">rabbitmq服务</param>
        /// <returns></returns>
        [HttpGet("Publish"), AllowAnonymous]
        public async Task<Result> Publish([FromServices] IRabbitMqAppService rabbitMqAppService)
        {
            await rabbitMqAppService.Pulish();
            return Result.Ok();
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <param name="rabbitMqAppService">rabbitmq服务</param>
        /// <returns></returns>
        [HttpGet("PublishError"), AllowAnonymous]
        public async Task<Result> PublishError([FromServices] IRabbitMqAppService rabbitMqAppService)
        {
            await rabbitMqAppService.PulishError();
            return Result.Ok();
        }
    }
}
