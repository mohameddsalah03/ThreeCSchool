using Microsoft.EntityFrameworkCore;
using ThreeCSchool.Core.Domain.Contracts.Persistence;
using ThreeCSchool.Core.Domain.Models.Base;
using ThreeCSchool.Infrastructure.Persistence.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Repos
{
    internal class GenericRepository<TEntity, TKey>(ThreeCDbContext _dbContext)
        : IGenericRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync(bool withTracking = false)
            => withTracking
                ? await _dbContext.Set<TEntity>().ToListAsync()
                : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id)
            => await _dbContext.Set<TEntity>().FindAsync(id);

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(
            ISpecifications<TEntity, TKey> spec, bool withTracking = false)
            => await ApplySpecifications(spec).ToListAsync();

        public async Task<TEntity?> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec)
            => await ApplySpecifications(spec).FirstOrDefaultAsync();

        public async Task<int> GetCountAsync(ISpecifications<TEntity, TKey> spec)
            => await ApplySpecifications(spec).CountAsync();

        public async Task AddAsync(TEntity entity)
            => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void Update(TEntity entity)
            => _dbContext.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity)
            => _dbContext.Set<TEntity>().Remove(entity);

        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity, TKey> spec)
            => SpecificationsEvaluator<TEntity, TKey>.GetQuery(_dbContext.Set<TEntity>(), spec);
    }
}