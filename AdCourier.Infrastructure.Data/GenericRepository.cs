using AdCourier.Context;
using AdCourier.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        
        private readonly SqlServerContext _sqlServerContext;
        public GenericRepository(SqlServerContext sqlServerContext)
        {
            _sqlServerContext = sqlServerContext ?? throw new ArgumentNullException(nameof(sqlServerContext));
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? skip = null, int? take = null)
        {
            IQueryable<T> query = _sqlServerContext.Set<T>();

            query = _sqlServerContext.Set<T>().AsNoTracking().Where(predicate);

            if (skip.HasValue)
            {
                query = query.Skip((skip.Value - 1) * take.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query;
        }

        public virtual async Task<IQueryable<T>> FindByAsyc(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? skip = null, int? take = null)
        {
            IQueryable<T> query = _sqlServerContext.Set<T>();

            query = _sqlServerContext.Set<T>().AsNoTracking().Where(predicate);

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
                //if (skip.Value.Equals(0))
                //{
                //    query = query.Skip(skip.Value * take.Value);
                //}
                //else
                //{
                //    query = query.Skip((skip.Value - 1) * take.Value);
                //}

            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await Task.FromResult(query);
        }


        public virtual async Task<int> FindByCountAsyc(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _sqlServerContext.Set<T>().AsNoTracking().Where(predicate);

            return await query.CountAsync();
        }
    }
}
