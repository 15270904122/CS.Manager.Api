using CS.Manager.Infrastructure.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Manager.Infrastructure.Filter
{
    /// <summary>
    /// 参数验证失败返回Result
    /// </summary>
    public class ValidateResultFilter : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var result = new ErrorResult(new SerializableError(context.ModelState), ResultCode.InvalidData);
                context.Result = new ObjectResult(result);
            }
        }
    }
}
