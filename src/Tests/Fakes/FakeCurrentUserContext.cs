using CoreApp.Application.Security;

namespace Tests.Fakes;

public sealed class FakeCurrentUserContext : ICurrentUserContext
{
    private readonly HashSet<string> _roles;

    public FakeCurrentUserContext(string? userId, string? userName, params string[] roles)
    {
        UserId = userId;
        UserName = userName;
        _roles = roles.ToHashSet();
    }

    public string? UserId { get; }
    public string? UserName { get; }
    public bool IsAuthenticated => UserId is not null;

    public bool IsInRole(string role) => _roles.Contains(role);
}
