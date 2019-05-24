using CS.Manager.Application.Auth.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CS.Manager.Infrastructure.Result;

namespace CS.Manager.Api.Authorization
{
    /// <summary>
    /// Token访问接口验证
    /// </summary>
    public class ApiTokenHandler : AuthenticationHandler<ApiTokenOptions>//, IAuthenticationRequestHandler
    {
        /// <inheritdoc />
        public ApiTokenHandler(IOptionsMonitor<ApiTokenOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) :
            base(options, logger, encoder, clock)
        {
        }


        /// <inheritdoc />
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = null;
            var schema = $"{ApiTokenOptions.Scheme} ";

            string header = Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(header) && header.StartsWith(schema, StringComparison.OrdinalIgnoreCase))
                token = header.Substring(schema.Length).Trim();

            if (string.IsNullOrEmpty(token))
            {
                token = Request.Query[ApiTokenOptions.Scheme];
            }

            if (!string.IsNullOrEmpty(token))
            {
                var service = Request.HttpContext.RequestServices.GetService<IAuthAppService>();
                var rs = await service.VerifyTokenAsync(token);
                if (!rs.Success)
                    return AuthenticateResult.Fail(rs.Message);

                return AuthenticateResult.Success(new AuthenticationTicket(rs.Data, ApiTokenOptions.Scheme));
            }

            return AuthenticateResult.Fail("Token不存在");
        }
    }
}
