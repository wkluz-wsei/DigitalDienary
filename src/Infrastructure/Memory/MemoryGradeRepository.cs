using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;

namespace Infrastructure.Memory;

public class MemoryGradeRepository : MemoryGenericRepository<Grade>, IGradeRepository
{
}
