﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Manager.Infrastructure.Page
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public interface IPageInfo
    {
        /// <summary>
        /// 页号，从 1 开始
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        int PageSize { get; set; }
    }

    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo : IPageInfo
    {
        /// <summary>
        /// 页号，从 1 开始
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 初始化 <see cref="PageInfo" /> 类的新实例。
        /// </summary>
        public PageInfo(int pageIndex = 1, int pageSize = 10)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
