using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;

namespace Infrastructure.Memory;

public class MemoryAcademicYearRepository : MemoryGenericRepository<AcademicYear>, IAcademicYearRepository
{
    public static readonly Guid AcademicYear2024Id = Guid.Parse("3bb4d12d-a507-46db-b676-f8d25d73cf59");
    public static readonly Guid AcademicYear2025Id = Guid.Parse("8e7a123e-b9b1-42d5-8bdd-c3cd7d7e96fb");

    public MemoryAcademicYearRepository()
    {
        _data.Add(AcademicYear2024Id, new AcademicYear
        {
            Id = AcademicYear2024Id,
            Name = "2024/2025"
        });

        _data.Add(AcademicYear2025Id, new AcademicYear
        {
            Id = AcademicYear2025Id,
            Name = "2025/2026"
        });
    }
}
