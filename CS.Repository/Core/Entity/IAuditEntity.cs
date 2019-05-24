using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Repository.Core.Entity
{
    public interface IAuditEntity<TPrimaryKey> : IEntity<TPrimaryKey>, ISoftDelete
    {
        DateTime CreationTime { get; set; }
        long? CreationUserId { get; set; }
        DateTime? LastModificationTime { get; set; }
        long? LastModificationUserId { get; set; }
    }
}
