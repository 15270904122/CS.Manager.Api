using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CS.Manager.Infrastructure.Filter
{
    /// <summary>
    /// 身份认证失败返回Result
    /// </summary>
    public class ValidateAuthorizedRequestFilter : ActionFilterAttribute
    {
        public ValidateAuthorizedRequestFilter()
        {
        }
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
        }

        /// <inheritdoc />
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Items.Add("ActionResult", context.Result);
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

        }
        //
        public override void OnResultExecuted(ResultExecutedContext context)
        {

        }
        //
        public override void OnResultExecuting(ResultExecutingContext context)
        {
        }
    }
}
