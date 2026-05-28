using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.Repositories;

public class EfLecturerRepository(UniversityDbContext context)
    : EfGenericRepository<Lecturer>(context.Lecturers), ILecturerRepository
{
}
