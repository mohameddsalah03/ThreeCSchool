using ThreeCSchool.Core.Domain.Models.Base;

namespace ThreeCSchool.Core.Domain.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity, TKey> GetRepo<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
            where TKey : IEquatable<TKey>;
        Task<int> SaveChangesAsync();
    }
}