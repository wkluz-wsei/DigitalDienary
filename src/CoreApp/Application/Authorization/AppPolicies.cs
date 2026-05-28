namespace CoreApp.Application.Authorization;

public enum AppPolicies
{
    AdminOnly,
    Administrator,
    DeanOfficeOnly,
    LecturerOrAdmin,
    ActiveUser,
    SalesDepartment
}

public static class AppPoliciesExtensions
{
    public static string Name(this AppPolicies policy)
    {
        return policy.ToString();
    }
}
