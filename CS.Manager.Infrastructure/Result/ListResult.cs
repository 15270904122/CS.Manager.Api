using CS.Manager.Infrastructure.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Manager.Infrastructure.Result
{
    /// <summary>
    /// 返回结果集
    /// </summary>
    public class ListResult<TData> : Result<IList<TData>>
    {
        /// <summary>
        /// 返回结果集
        /// </summary>
        public ListResult()
        {
            Data = new List<TData>();
        }

        /// <summary>
        /// 返回结果集
        /// </summary>
        public ListResult(IList<TData> data)
            : base(data)
        {
        }

        /// <summary>
        /// 返回结果集
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="message">提示信息</param>
        public ListResult(ResultCode code, string message = null)
            : base(code, message)
        {
            Data = new List<TData>();
        }
    }

    public static class ListResult
    {
        #region 静态函数

        /// <summary>
        /// 返回指定 Code
        /// </summary>
        public static ListResult<T> FromCode<T>(ResultCode code, string message = null)
        {
            return new ListResult<T>(code, message);
        }

        /// <summary>
        /// 返回异常信息
        /// </summary>
        public static ListResult<T> FromError<T>(string message, ResultCode code = ResultCode.Fail)
        {
            return new ListResult<T>(code, message);
        }

        /// <summary>
        /// 返回新结果
        /// </summary>
        public static ListResult<T> FromResult<T>(Result result)
        {
            return new ListResult<T>(result.Code, result.Message);
        }

        /// <summary>
        /// 返回新结果
        /// </summary>
        public static ListResult<T> FromResult<T>(Result result, IList<T> data)
        {
            return new ListResult<T>(result.Code, result.Message) { Data = data };
        }

        /// <summary>
        /// 返回新结果
        /// </summary>
        public static ListResult<T> FromResult<T>(Result<IList<T>> result)
        {

            return new ListResult<T>(result.Code, result.Message) { Data = result.Data };
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        public static ListResult<T> FromData<T>(IList<T> data)
        {
            return new ListResult<T>(data);
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        public static ListResult<T> Ok<T>(IList<T> data)
        {
            return FromData(data);
        }

        #endregion
    }
}
