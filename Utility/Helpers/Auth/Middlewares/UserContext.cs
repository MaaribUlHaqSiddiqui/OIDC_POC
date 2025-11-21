using Helpers.Auth.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Auth.Middlewares
{
    public interface IUserContext { UserPayload Data { get; set; } }

    public class UserContext : IUserContext { public UserPayload Data { get; set; } }

    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next) { _next = next; }

        public async Task InvokeAsync(HttpContext context, IUserContext userContext)
        {
            userContext.Data = HTTPContextUserRetriever.GetUserPayloadFromClaims(context.User);
            await _next(context);
        }
    }
}
