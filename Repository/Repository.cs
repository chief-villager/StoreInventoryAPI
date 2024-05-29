using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace storeInventoryApi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task CreateAsync(T entity, CancellationToken cancellationToken)
        {
            _ = entity == null
                    ? throw new NullReferenceException("entity cannot be null")
                    : await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task DeleteAsync(Guid Id, CancellationToken cancellationToken)
        {
            T? entity = Id == Guid.Empty ? throw new ArgumentOutOfRangeException(nameof(Id), "does not exist") : await _dbSet.FindAsync(Id, cancellationToken);
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            _dbSet.Remove(entity!);
            await _context.SaveChangesAsync(cancellationToken);

        }

        public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<T> GetAsync(Guid Id, CancellationToken cancellationToken)
        {
            T? entity = Id == Guid.Empty
                            ? throw new ArgumentOutOfRangeException(nameof(Id), "does not exist")
                            : await _dbSet.FindAsync(Id, cancellationToken)
                            ?? throw new ArgumentNullException(nameof(entity));
            return entity;

        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}