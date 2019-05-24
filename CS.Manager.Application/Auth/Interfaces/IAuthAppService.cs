using CS.Manager.Dto.Auth;
using CS.Manager.Infrastructure.Result;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CS.Manager.Application.Auth.Interfaces
{
    /// <summary>
    /// 身份认证应用服务
    /// </summary>
    public interface IAuthAppService
    {
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        Task<Result<LoginOutput>> LoginAsync(LoginInput loginInput);

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="loginInput"></param>
        /// <returns></returns>
        Task<Result<TokenInfo>> CreateTokenAsync(LoginInput loginInput);

    }
}
