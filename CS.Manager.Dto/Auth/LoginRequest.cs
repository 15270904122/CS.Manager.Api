using AutoMapper;
using AutoMapper.Configuration.Conventions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CS.Manager.Dto.Auth
{
    /// <summary>
    /// 登陆接口请求参数
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
    }

    /// <summary>
    /// 登陆接口用户类型请求参数
    /// </summary>
    public enum LoginUserTypeRequest
    {
        /// <summary>
        /// 管理员
        /// </summary>
        [Display(Name = "管理员")]
        Admin=1,

        /// <summary>
        /// 学生
        /// </summary>
        [Display(Name = "学生")]
        Student = 2
    }
}
