using CS.Manager.Application.Auth.Interfaces;
using CS.Manager.Dto.Auth;
using CS.Manager.Infrastructure.Result;
using CS.Repository.Core.DbType;
using CS.Repository.Core.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CS.Manager.Infrastructure.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CS.Manager.Application.Auth
{
    /// <summary>
    /// 登陆认证应用服务
    /// </summary>
    public class AuthAppService : IAuthAppService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        private readonly IFreeSql<MasterDb> _masterFreeSql;
        public AuthAppService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _masterFreeSql = serviceProvider.GetService<IFreeSql<MasterDb>>();
            _configuration = configuration;

        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        public async Task<Result<LoginOutput>> LoginAsync(LoginInput loginInput)
        {
            var user = await _masterFreeSql.GetRepository<Users, long>().Where(x => x.UserName == loginInput.UserName && x.Password == loginInput.Password).ToOneAsync();
            if (user is null)
            {
                return Result.FromCode<LoginOutput>(ResultCode.LoginFailed);
            }
            var expireInterval = _configuration.GetSection("TokenExpireInterval").Value;
            var hour = int.TryParse(expireInterval, out int val) ? val : 1;
            var userInfo = user.MapTo<UserInfo>();
            var token = Guid.NewGuid().ToString("N");
            var tokenInfo = new TokenInfo
            {
                ExpireTime = DateTime.Now.AddHours(hour),
                Token = token,
            };
            var result = new LoginOutput
            {
                TokenInfo = tokenInfo,
                UserInfo = userInfo
            };
            await RedisHelper.SetAsync(token, result, hour * 60 * 60);
            return Result.FromData(result);
        }

        /// <summary>
        /// 生成Tokne
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        public async Task<Result<TokenInfo>> CreateTokenAsync(LoginInput loginInput)
        {
            var user = await _masterFreeSql.GetRepository<Users, long>().Where(x => x.UserName == loginInput.UserName && x.Password == loginInput.Password).ToOneAsync();
            if (user is null)
            {
                return Result.FromCode<TokenInfo>(ResultCode.LoginFailed);
            }
            var expireInterval = _configuration.GetSection("TokenExpireInterval").Value;
            var hour = int.TryParse(expireInterval, out int val) ? 1 : val;
            var userInfo = user.MapTo<UserInfo>();
            var token = Guid.NewGuid().ToString("N");
            var tokenInfo = new TokenInfo
            {
                ExpireTime = DateTime.Now.AddHours(hour),
                Token = token,
            };
            var result = new LoginOutput
            {
                TokenInfo = tokenInfo,
                UserInfo = userInfo
            };
            await RedisHelper.SetAsync(token, result, hour * 60 * 60);
            return Result.FromData(tokenInfo);
        }
    }
}
