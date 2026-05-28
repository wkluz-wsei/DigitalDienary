using CoreApp.Application.Paging;
using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfGenericRepository<T>(DbSet<T> set) : IGenericRepositoryAsync<T>
    where T : EntityBase
{
    protected DbSet<T> Set { get; } = set;

    public virtual async Task<T?> FindByIdAsync(Guid id)
    {
        return await Set.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> FindAllAsync()
    {
        return await Set.ToListAsync();
    }

    public virtual async Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        var items = await Set
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>(
            items,
            await Set.CountAsync(),
            page,
            pageSize);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        var entry = await Set.AddAsync(entity);
        return entry.Entity;
    }

    public virtual Task<T> UpdateAsync(T entity)
    {
        var entityEntry = Set.Update(entity);
        return Task.FromResult(entityEntry.Entity);
    }

    public virtual async Task RemoveByIdAsync(Guid id)
    {
        var entity = await Set.FindAsync(id);
        if (entity is null)
        {
            return;
        }

        Set.Remove(entity);
    }
}
