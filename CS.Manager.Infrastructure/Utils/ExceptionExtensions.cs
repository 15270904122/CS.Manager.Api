using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;

namespace CS.Manager.Infrastructure.Utils
{
    /// <summary>
    /// 异常扩展
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// 获取异常的原始异常信息(<see cref="System.Exception.InnerException"/>)
        /// </summary>
        public static Exception GetOriginalException(this Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            while (exception.InnerException != null)
                exception = exception.InnerException;

            return exception;
        }

        /// <summary>
        /// 使用 <see cref="ExceptionDispatchInfo.Throw(Exception)"/> 方法重抛异常
        /// 保留堆栈信息
        /// </summary>
        public static void ReThrow(this Exception exception)
        {
            ExceptionDispatchInfo.Throw(exception);
        }
    }
}
