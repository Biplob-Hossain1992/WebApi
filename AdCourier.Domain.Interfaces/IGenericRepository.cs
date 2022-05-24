using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdCourier.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
             int? skip = null, int? take = null);

        Task<IQueryable<T>> FindByAsyc(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
             int? skip = null, int? take = null);

        Task<int> FindByCountAsyc(Expression<Func<T, bool>> predicate);
    }
}
