using Microsoft.EntityFrameworkCore;
using ThreeCSchool.Core.Domain.Contracts.Persistence;
using ThreeCSchool.Core.Domain.Models.Base;

namespace ThreeCSchool.Infrastructure.Persistence.Repos
{
    public static class SpecificationsEvaluator<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public static IQueryable<TEntity> GetQuery(
            IQueryable<TEntity> dbSet,
            ISpecifications<TEntity, TKey> spec)
        {
            var query = dbSet;

            if (spec.Criteria is not null)
                query = query.Where(spec.Criteria);

            if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);
            else if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);

            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            query = spec.Includes.Aggregate(query, (current, include) =>
                current.Include(include));

            query = spec.ThenIncludeStrings.Aggregate(query, (current, include) =>
                current.Include(include));

            return query;
        }
    }
}