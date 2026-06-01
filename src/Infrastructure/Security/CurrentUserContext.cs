using System.Security.Claims;
using CoreApp.Application.Security;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security;

public class CurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
{
    private HttpContext? HttpContext => httpContextAccessor.HttpContext;

    public string? UserId => HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserName => HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

    public bool IsAuthenticated => HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) => HttpContext?.User?.IsInRole(role) ?? false;
}
