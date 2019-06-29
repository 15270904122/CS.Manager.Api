using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CS.Manager.Infrastructure.Jobs
{
    /// <summary>
    /// Job管理
    /// </summary>
    public interface IJobManager
    {
        /// <summary>
        /// 把Job任务加入到队列
        /// </summary>
        string Enqueue<TJob, TArgs>(TArgs args, TimeSpan? delay = null, string continueWith = null)
            where TJob : IJob<TArgs>;

        /// <summary>
        /// 把Job任务加入到队列
        /// </summary>   
        string Enqueue<TJob>(Expression<Func<TJob, Task>> methodCall, TimeSpan? delay = null, string continueWith = null);

        /// <summary>
        /// 把Job从队列中删除
        /// </summary>
        bool Delete(string jobId);
    }
}
