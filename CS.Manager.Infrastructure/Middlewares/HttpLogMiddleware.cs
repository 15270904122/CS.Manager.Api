using CS.Manager.Infrastructure.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CS.Manager.Infrastructure.Middlewares
{
    /// <summary>
    /// 请求日志
    /// </summary>
    public class HttpLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        private readonly JsonSerializerSettings _serializerSettings;
        private readonly IList<MediaTypeHeaderValue> _logMediaTypes;

        /// <inheritdoc />
        public HttpLogMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _loggerFactory = loggerFactory;

            _logMediaTypes = new List<MediaTypeHeaderValue>()
            {
                new MediaTypeHeaderValue("text/json"),
                new MediaTypeHeaderValue("text/xml"),
                new MediaTypeHeaderValue("application/json"),
                new MediaTypeHeaderValue("application/json-patch+json"),
                new MediaTypeHeaderValue("application/*+json"),
                new MediaTypeHeaderValue("application/x-www-form-urlencoded"),
            };

            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Include,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                Converters =
                {
                    new IsoDateTimeConverter
                    {
                        DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fffffff",
                        DateTimeStyles = DateTimeStyles.AssumeLocal
                    }
                }
            };
        }

        /// <summary>
        /// 管道处理
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var headers = context.Request.GetTypedHeaders();
            var requestBody = string.Empty;
            var responseBody = string.Empty;
            Exception exception = null;

            var watch = Stopwatch.StartNew();

            try
            {
                if (headers.ContentType != null && _logMediaTypes.Any(p => headers.ContentType.IsSubsetOf(p)))
                {
                    if (!context.Request.Body.CanSeek)
                    {
                        context.Request.EnableBuffering();
                        await request.Body.DrainAsync(CancellationToken.None);
                        request.Body.Seek(0L, SeekOrigin.Begin);
                    }

                    var encoding = headers.ContentType.Encoding ?? SelectCharacterEncoding(request.ContentType);
                    using (var reader = new StreamReader(request.Body, encoding, false, 4096, true))
                    {
                        requestBody = await reader.ReadToEndAsync();
                    }

                    request.Body.Seek(0L, SeekOrigin.Begin);
                }

                await _next(context);
                watch.Stop();
            }
            catch (Exception e)
            {
                exception = e;
                e.ReThrow();
            }
            finally
            {
                watch.Stop();

                if (context.Items.TryGetValue("ActionDescriptor", out var c1) &&
                    c1 is ControllerActionDescriptor actionDescriptor)
                {
                    var logName = $"{actionDescriptor.ControllerTypeInfo.FullName}.{actionDescriptor.MethodInfo.Name}";
                    var logger = _loggerFactory.CreateLogger(logName);

                    var result = context.Items.TryGetValue("ActionResult", out var c3) ? (IActionResult)c3 : null;
                    if (result is ObjectResult objectResult)
                    {
                        responseBody = JsonConvert.SerializeObject(objectResult.Value, _serializerSettings);
                    }

                    var auth = request.Headers["Authorization"];
                    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var userName = context.User.FindFirst(ClaimTypes.Name)?.Value;

                    var logLevel = exception != null ? LogLevel.Error : LogLevel.Information;

                    logger.Log(logLevel, exception,
                        "请求：{Protocol} {Method} {RequestUrl} {RequestContentType}\r\n" +
                        "用户：UserName={UserName} UserId={UserId} Token={Authorization}\r\n" +
                        "{RequestBody}\r\n" +
                        "响应：{ElapsedMilliseconds}ms {StatusCode} {ResponseContentType}\r\n" +
                        "{ResponseBody}\r\n",
                        context.Request.Protocol,
                        context.Request.Method,
                        context.Request.GetDisplayUrl(),
                        headers.ContentType,
                        userName, userId, auth,
                        requestBody,
                        watch.Elapsed.TotalMilliseconds,
                        context.Response.StatusCode,
                        context.Response.ContentType,
                        responseBody);
                }
            }
        }



        private Encoding SelectCharacterEncoding(string contentType)
        {
            var mediaType = contentType == null ? new MediaType() : new MediaType(contentType);

            return mediaType.Encoding ?? Encoding.ASCII;
        }

    }


    /// <inheritdoc />
    public class ActionLogFilter : ActionFilterAttribute
    {
        /// <inheritdoc />
        public ActionLogFilter()
        {
            Order = -1000000;
        }


        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items.Add("ActionDescriptor", context.ActionDescriptor);
        }

        /// <inheritdoc />
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Items.Add("ActionResult", context.Result);
        }
    }
}
