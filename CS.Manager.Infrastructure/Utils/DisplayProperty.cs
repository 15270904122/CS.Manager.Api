using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Manager.Infrastructure.Utils
{
    /// <summary>
    /// 定义 <see cref="DisplayAttribute"/> 的属性
    /// </summary>
    public enum DisplayProperty
    {
        /// <summary>
        /// 名称
        /// </summary>
        Name,

        /// <summary>
        /// 短名称
        /// </summary>
        ShortName,

        /// <summary>
        /// 分组名称
        /// </summary>
        GroupName,

        /// <summary>
        /// 说明
        /// </summary>
        Description,

        /// <summary>
        /// 排序
        /// </summary>
        Order,

        /// <summary>
        /// 水印信息
        /// </summary>
        Prompt,
    }
}
