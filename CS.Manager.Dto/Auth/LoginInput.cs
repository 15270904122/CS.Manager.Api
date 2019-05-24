using AutoMapper;
using AutoMapper.Configuration.Conventions;
using CS.Manager.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Manager.Dto.Auth
{
    /// <summary>
    /// 登录入参
    /// </summary>
    public class LoginInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
