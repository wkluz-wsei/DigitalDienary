using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.Repositories;

public class EfCourseRepository(UniversityDbContext context)
    : EfGenericRepository<Course>(context.Courses), ICourseRepository
{
}
