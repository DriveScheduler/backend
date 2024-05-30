using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace API.Authentication
{
    [Authorize]
    public class JwtControllerBase : ControllerBase
    {
        protected Guid GetUserId()
        {
            string? value = User.FindFirstValue(JwtProvider.CLAIM_ID);
            if (string.IsNullOrEmpty(value)) return Guid.Empty;
            return Guid.Parse(value);
        }
    }
}
