using System.Linq.Expressions;
using CarnetSante.Core.Models;
using CarnetSante.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CarnetSante.Data.Repositories;

/// <summary>
/// Implémentation générique du Repository Pattern avec soft-delete.
/// </summary>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly CarnetSanteDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(CarnetSanteDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.Where(e => !e.IsDeleted).ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).Where(e => !e.IsDeleted).ToListAsync();

    public async Task<T> AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
        => await _dbSet.AnyAsync(e => e.Id == id && !e.IsDeleted);

    public async Task<int> CountAsync()
        => await _dbSet.CountAsync(e => !e.IsDeleted);

    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        => await _dbSet.Where(predicate).CountAsync(e => !e.IsDeleted);
}
