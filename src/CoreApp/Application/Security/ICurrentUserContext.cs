namespace CoreApp.Application.Security;

public interface ICurrentUserContext
{
    string? UserId { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}
