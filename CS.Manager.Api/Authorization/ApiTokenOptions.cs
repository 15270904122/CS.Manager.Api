using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Manager.Api.Authorization
{
    /// <summary>
    /// Token访问接口配置
    /// </summary>
    public class ApiTokenOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// 验证方案
        /// </summary>
        public const string Scheme = "Token";
    }
}
