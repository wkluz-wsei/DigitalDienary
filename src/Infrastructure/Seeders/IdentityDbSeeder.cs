using CoreApp.Application.Security;
using Infrastructure.EntityFramework.Entities;
using Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seeders;

public class IdentityDbSeeder : IDataSeeder
{
    public int Order => 1;

    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ILogger<IdentityDbSeeder> _logger;

    public IdentityDbSeeder(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        ILogger<IdentityDbSeeder> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedUsersAsync();
    }

    private async Task SeedRolesAsync()
    {
        var roles = new[]
        {
            new AppRole(UserRole.Administrator.ToString(), "Pełny dostęp do systemu."),
            new AppRole(UserRole.DeanOfficeStaff.ToString(), "Pracownik dziekanatu."),
            new AppRole(UserRole.Lecturer.ToString(), "Prowadzący zajęcia."),
            new AppRole(UserRole.Student.ToString(), "Student uczelni.")
        };

        foreach (var role in roles)
        {
            if (await _roleManager.RoleExistsAsync(role.Name!))
                continue;

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                _logger.LogError("Błąd tworzenia roli {Role}: {Errors}", role.Name, FormatErrors(result));
            }
        }
    }

    private async Task SeedUsersAsync()
    {
        var users = new[]
        {
            new SeedUser("F5BADE14-6CC8-42A2-9A44-9842DA2D9280", "admin@app.pl", "Adam", "Administrator", "IT", "Admin@123!", UserRole.Administrator),
            new SeedUser("93A7FFDD-057F-4021-9C68-FE06951FFA65", "jan.kowalski@app.pl", "Jan", "Kowalski", "Sales", "Manager@123!", UserRole.DeanOfficeStaff),
            new SeedUser("3D4769E2-1C75-43E1-A5BB-1F71C68E9F57", "anna.nowak@app.pl", "Anna", "Nowak", "Sales", "Lecturer@123!", UserRole.Lecturer),
            new SeedUser("0E136AB2-1A6A-4A16-938D-84DFB0F64BBA", "piotr.wisniewski@app.pl", "Piotr", "Wiśniewski", "Sales", "Piotr123!", UserRole.Lecturer),
            new SeedUser("76B253D6-C16C-470A-943C-92F314A090F2", "maria.wojcik@app.pl", "Maria", "Wójcik", "Support", "Student@123!", UserRole.Student),
            new SeedUser("E90A39C9-9CE2-400A-8A7B-8CF300D3B292", "tomasz.kaminski@app.pl", "Tomasz", "Kamiński", "Management", "DeanOffice@123!", UserRole.DeanOfficeStaff)
        };

        foreach (var seedUser in users)
        {
            await CreateUserAsync(seedUser);
        }
    }

    private async Task CreateUserAsync(SeedUser seedUser)
    {
        if (await _userManager.FindByEmailAsync(seedUser.Email) is not null)
        {
            _logger.LogInformation("Użytkownik {Email} już istnieje — pomijam.", seedUser.Email);
            return;
        }

        var user = new AppUser
        {
            Id = seedUser.Id,
            UserName = seedUser.Email,
            NormalizedUserName = seedUser.Email.ToUpperInvariant(),
            NormalizedEmail = seedUser.Email.ToUpperInvariant(),
            Email = seedUser.Email,
            FullName = $"{seedUser.FirstName} {seedUser.LastName}",
            EmailConfirmed = true,
            LockoutEnabled = true,
            LockoutEnd = null,
            AccessFailedCount = 0,
            TwoFactorEnabled = false,
            PhoneNumberConfirmed = false,
            FirstName = seedUser.FirstName,
            LastName = seedUser.LastName,
            Department = seedUser.Department,
            Status = SystemUserStatus.Inactive,
            CreatedAt = DateTime.UtcNow
        };

        user.Activate();

        var createResult = await _userManager.CreateAsync(user, seedUser.Password);
        if (!createResult.Succeeded)
        {
            _logger.LogError("Błąd tworzenia użytkownika {Email}: {Errors}", user.Email, FormatErrors(createResult));
            return;
        }

        var roleResult = await _userManager.AddToRoleAsync(user, seedUser.Role.ToString());
        if (!roleResult.Succeeded)
        {
            _logger.LogError("Błąd przypisania roli {Role} dla {Email}: {Errors}", seedUser.Role, seedUser.Email, FormatErrors(roleResult));
            return;
        }

        _logger.LogInformation("Utworzono użytkownika {Email} z rolą {Role}.", seedUser.Email, seedUser.Role);
    }

    private static string FormatErrors(IdentityResult result) =>
        string.Join("; ", result.Errors.Select(e => e.Description));
}

internal record SeedUser(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    string Department,
    string Password,
    UserRole Role
);
