using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Manager.Dto.Auth
{
    /// <summary>
    /// 身份令牌信息
    /// </summary>
    public class TokenInfo
    {
        /// <summary>
        /// 身份令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 过期时间（绝对时间）
        /// </summary>
        public DateTime ExpireTime { get; set; }
    }
}
