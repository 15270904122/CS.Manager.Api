using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CS.Manager.Infrastructure.Jobs
{
    /// <summary>
    /// 定义一个Job
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public interface IJob<in TArgs>
    {
        /// <summary>
        /// 执行Job任务
        /// </summary>
        Task Execute(TArgs args);
    }

    /// <summary>
    /// Job 基类
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public abstract class Job<TArgs> : IJob<TArgs>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public Job()
        {
            Logger = NullLogger.Instance;
        }
        /// <summary>
        /// 执行Job任务
        /// </summary>
        public abstract Task Execute(TArgs args);

        /// <summary>
        /// 日志
        /// </summary>
        public ILogger Logger { get; set; }
    }
}
