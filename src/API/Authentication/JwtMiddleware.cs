using Microsoft.IdentityModel.Tokens;

using System.Security.Claims;

namespace API.Authentication
{
    public class JwtMiddleware(JwtProvider tokenProvider) : IMiddleware
    {
        private readonly JwtProvider _tokenProvider = tokenProvider;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!token.IsNullOrEmpty())
            {
                try
                {                   
                    var claimsPrincipal = _tokenProvider.ConvertToken(token);                 
                    var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;                    
                    context.Items["UserId"] = userId;                    
                }
                catch (Exception)
                {                                 
                    throw new UnauthorizedAccessException();
                }
            }            
            await next(context);
        }
    }
}