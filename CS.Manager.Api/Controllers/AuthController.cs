﻿using CS.Manager.Application.Auth.Interfaces;
using CS.Manager.Dto.Auth;
using CS.Manager.Infrastructure.Jobs;
using CS.Manager.Infrastructure.Result;
using CS.Manager.Infrastructure.Utils;
using CS.Manager.Job.BackgroundJobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CS.Manager.Api.Controllers
{
    /// <summary>
    /// 用户认证
    /// </summary>
    public class AuthController : ApiController
    {
        public AuthController()
        {
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="loginRequest">请求参数</param>
        /// <param name="authAppService">登陆服务</param>
        /// <returns></returns>
        [HttpPost("Login"), AllowAnonymous]
        public async Task<Result<LoginOutput>> Login([FromBody, Required]LoginRequest loginRequest, [FromServices] IAuthAppService authAppService)
        {
            return await authAppService.LoginAsync(loginRequest.MapTo<LoginInput>());
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="loginRequest">请求参数</param>
        /// <param name="authAppService">登陆服务</param>
        /// <returns></returns>
        [HttpPost("GetToken"), AllowAnonymous]
        public async Task<Result<TokenInfo>> CreateToken([FromBody, Required]LoginRequest loginRequest, [FromServices] IAuthAppService authAppService)
        {
            return await authAppService.CreateTokenAsync(loginRequest.MapTo<LoginInput>());
        }

        /// <summary>
        /// 测试Job
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestJob"), AllowAnonymous]
        public async Task<Result> TestJob([FromServices] IJobManager jobManager)
        {
            jobManager.Enqueue<Demo1, Demo1Args>(new Demo1Args { Name = "1" });
            return Result.Ok();
        }
    }
}
