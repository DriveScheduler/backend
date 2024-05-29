using Microsoft.IdentityModel.Tokens;

using System.Security.Claims;

namespace API.Authentication
{
    public class JwtMiddleware(JwtTokenProvider tokenProvider) : IMiddleware
    {
        private readonly JwtTokenProvider _tokenProvider = tokenProvider;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!token.IsNullOrEmpty())
            {
                try
                {
                    // Verify the token using the JwtSecurityTokenHandlerWrapper
                    var claimsPrincipal = _tokenProvider.ConvertToken(token);

                    // Extract the user ID from the token
                    var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    // Store the user ID in the HttpContext items for later use
                    context.Items["UserId"] = userId;

                    // You can also do the for same other key which you have in JWT token.
                }
                catch (Exception)
                {
                    // If the token is invalid, throw an exception                    
                }


            }
            // Continue processing the request
            await next(context);
        }
    }
}
