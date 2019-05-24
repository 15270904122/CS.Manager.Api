using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CS.Manager.Infrastructure.Result
{
    /// <summary>
    /// Api请求状态码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 操作成功
        ///</summary>
        [Display(Name = "操作成功", GroupName = Result.SuccessCode)]
        Ok = 0,

        /// <summary>
        /// 操作失败
        ///</summary>
        [Display(Name = "操作失败")]
        Fail = 1,

        /// <summary>
        /// 服务数据异常
        ///</summary>
        [Display(Name = "服务数据异常")]
        ServerError = 10,

        /// <summary>
        /// Token 失效
        ///</summary>
        [Display(Name = "Token 失效")]
        InvalidToken = 20,

        /// <summary>
        /// 未登录
        ///</summary>
        [Display(Name = "未登录")]
        Unauthorized = 21,

        /// <summary>
        /// 未授权
        /// </summary>
        [Display(Name = "未授权")]
        Forbidden = 22,

        /// <summary>
        /// 用户名密码错误
        /// </summary>
        [Display(Name = "用户名密码错误")]
        LoginFailed = 23,

        /// <summary>
        /// 参数无效
        /// </summary>
        [Display(Name = "参数无效")]
        InvalidData = 403,

        /// <summary>
        /// 没有此条记录
        ///</summary>
        [Display(Name = "没有此条记录")]
        NoRecord = 404,

        /// <summary>
        /// 重复记录
        /// </summary>
        [Display(Name = "已有记录，请勿重复操作")]
        DuplicateRecord = 405,
    }
}
