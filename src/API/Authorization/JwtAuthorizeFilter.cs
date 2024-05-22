using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using System.Security.Claims;

namespace API.Authorization
{
    public class JwtAuthorizeFilter(JwtTokenProvider tokenProvider) : IAuthorizationFilter
    {
        private readonly JwtTokenProvider _tokenProvider = tokenProvider;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var hasAuthorizeAttribute = context.ActionDescriptor.EndpointMetadata
           .Any(em => em is AuthorizeAttribute);

            if (hasAuthorizeAttribute)
            {
                var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        // Validate the token and extract claims
                        var claimsPrincipal = tokenProvider.ConvertToken(token);

                        // Extract the user ID from the token
                        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        context.HttpContext.Items["UserId"] = userId;
                    }
                    catch (Exception)
                    {
                        context.Result = new UnauthorizedResult();
                    }
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}
