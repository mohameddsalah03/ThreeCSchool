using ThreeCSchool.Core.Domain.Contracts.Persistence;
using ThreeCSchool.Core.Domain.Models.Base;
using ThreeCSchool.Infrastructure.Persistence.Data;

namespace ThreeCSchool.Infrastructure.Persistence.Repos
{
    public class UnitOfWork(ThreeCDbContext _context) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repos = new();

        public IGenericRepository<TEntity, TKey> GetRepo<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            var name = typeof(TEntity).Name;

            if (_repos.ContainsKey(name))
                return (IGenericRepository<TEntity, TKey>)_repos[name];

            var repo = new GenericRepository<TEntity, TKey>(_context);
            _repos[name] = repo;
            return repo;
        }

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}