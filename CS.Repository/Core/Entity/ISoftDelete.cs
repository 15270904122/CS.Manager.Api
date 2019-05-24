using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Repository.Core.Entity
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletionTime { get; set; }
    }
}
