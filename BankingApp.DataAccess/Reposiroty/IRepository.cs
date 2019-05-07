using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BankingApp.DataAccess.Reposiroty
{
    public interface IRepository<T> where T : class
    {
        T GetById(Guid id);
        void Create(T item);
        void Update(T item);
        IList<T> Get( Expression<Func<T, bool>> filter = null,
                      Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                      params Expression<Func<T, object>>[] includes);
    }
}