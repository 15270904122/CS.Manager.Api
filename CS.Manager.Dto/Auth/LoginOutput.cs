using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Manager.Dto.Auth
{
    /// <summary>
    /// 登陆出参
    /// </summary>
    public class LoginOutput
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// 身份令牌信息
        /// </summary>
        public TokenInfo TokenInfo { get; set; }
    }
}
