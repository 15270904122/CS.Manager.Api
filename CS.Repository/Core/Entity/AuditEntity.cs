using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Repository.Core.Entity
{
    public class AuditEntity<TPrimaryKey> : IAuditEntity<TPrimaryKey>
    {
        [Column(IsPrimary = true, IsIdentity = true)]
        public TPrimaryKey Id { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreationUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModificationUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
