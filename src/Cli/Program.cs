using CoreApp.Application.Security;
using Infrastructure.EntityFramework.Context;
using Infrastructure.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

if (args.Length < 2)
{
    Console.WriteLine("Usage: dotnet run -- --username <email> --password <password>");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  --username <email>     User email (used as username)");
    Console.WriteLine("  --password <password>  User password");
    Console.WriteLine("  --db <path>            Path to SQLite database file (default: university.db)");
    return 1;
}

string? username = null;
string? password = null;
string dbPath = "university.db";

for (var i = 0; i < args.Length; i++)
{
    switch (args[i])
    {
        case "--username" when i + 1 < args.Length:
            username = args[++i];
            break;
        case "--password" when i + 1 < args.Length:
            password = args[++i];
            break;
        case "--db" when i + 1 < args.Length:
            dbPath = args[++i];
            break;
    }
}

if (string.IsNullOrWhiteSpace(username))
{
    Console.Error.WriteLine("Error: --username is required.");
    return 1;
}

if (string.IsNullOrWhiteSpace(password))
{
    Console.Error.WriteLine("Error: --password is required.");
    return 1;
}

var serviceProvider = BuildServiceProvider(dbPath);

using var scope = serviceProvider.CreateScope();

var dbContext = scope.ServiceProvider.GetRequiredService<UniversityDbContext>();
await dbContext.Database.MigrateAsync();

var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

await EnsureRolesExistAsync(roleManager);

var firstName = PromptRequired("First name: ");
var lastName = PromptRequired("Last name: ");
var department = PromptRequired("Department: ");
var role = PromptRole();

var createResult = await CreateUserAsync(userManager, username, password, firstName, lastName, department, role);

return createResult ? 0 : 1;

static IServiceProvider BuildServiceProvider(string dbPath)
{
    var services = new ServiceCollection();
    services.AddLogging();

    var fullPath = Path.GetFullPath(dbPath);

    services.AddDbContext<UniversityDbContext>(options =>
        options.UseSqlite($"Data Source={fullPath}"));

    services.AddIdentity<AppUser, AppRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<UniversityDbContext>()
    .AddDefaultTokenProviders();

    return services.BuildServiceProvider();
}

static async Task EnsureRolesExistAsync(RoleManager<AppRole> roleManager)
{
    foreach (var role in Enum.GetNames<UserRole>())
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new AppRole(role));
        }
    }
}

static string PromptRequired(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        var value = Console.ReadLine()?.Trim();
        if (!string.IsNullOrWhiteSpace(value))
            return value;

        Console.Error.WriteLine("This field is required.");
    }
}

static UserRole PromptRole()
{
    var roles = Enum.GetValues<UserRole>();

    Console.WriteLine();
    Console.WriteLine("Select a role:");

    for (var i = 0; i < roles.Length; i++)
    {
        Console.WriteLine($"  {i + 1} - {roles[i]}");
    }

    Console.WriteLine();

    while (true)
    {
        Console.Write("Enter role number: ");
        var input = Console.ReadLine()?.Trim();

        if (int.TryParse(input, out var number) && number >= 1 && number <= roles.Length)
        {
            return roles[number - 1];
        }

        Console.Error.WriteLine($"Invalid selection. Enter a number between 1 and {roles.Length}.");
    }
}

static async Task<bool> CreateUserAsync(
    UserManager<AppUser> userManager,
    string email,
    string password,
    string firstName,
    string lastName,
    string department,
    UserRole role)
{
    if (await userManager.FindByEmailAsync(email) is not null)
    {
        Console.Error.WriteLine($"Error: User '{email}' already exists.");
        return false;
    }

    var user = new AppUser
    {
        UserName = email,
        NormalizedUserName = email.ToUpperInvariant(),
        NormalizedEmail = email.ToUpperInvariant(),
        Email = email,
        EmailConfirmed = true,
        LockoutEnabled = true,
        LockoutEnd = null,
        AccessFailedCount = 0,
        TwoFactorEnabled = false,
        PhoneNumberConfirmed = false,
        FirstName = firstName,
        LastName = lastName,
        FullName = $"{firstName} {lastName}",
        Department = department,
        Status = SystemUserStatus.Active,
        CreatedAt = DateTime.UtcNow
    };

    var createResult = await userManager.CreateAsync(user, password);
    if (!createResult.Succeeded)
    {
        var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
        Console.Error.WriteLine($"Error creating user: {errors}");
        return false;
    }

    var roleResult = await userManager.AddToRoleAsync(user, role.ToString());
    if (!roleResult.Succeeded)
    {
        var errors = string.Join("; ", roleResult.Errors.Select(e => e.Description));
        Console.Error.WriteLine($"User created but role assignment failed: {errors}");
        return false;
    }

    Console.WriteLine($"User '{email}' created with role '{role}'.");
    return true;
}
