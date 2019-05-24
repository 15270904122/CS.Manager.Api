using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Repository.Core.Entity
{
    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Column(IsPrimary = true, IsIdentity = true)]
        public TPrimaryKey Id { get; set; }
    }
}
