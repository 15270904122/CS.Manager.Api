using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Repository.Core.Entity
{
    public interface IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        TPrimaryKey Id { get; set; }
    }
}
