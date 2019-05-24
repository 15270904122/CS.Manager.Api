using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Manager.Dto.Auth
{
    public class LoginResponse
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
