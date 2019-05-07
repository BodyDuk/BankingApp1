using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BankingApp.DataAccess.Reposiroty
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected BankContext db;
        protected DbSet<T> dbSet;

        public RepositoryBase(BankContext context)
        {
            db = context;
            dbSet = context.Set<T>();
        }

        public void Create(T item) => db.Add(item);
        
        public virtual IList<T> Get(Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, 
            IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dbSet;

            foreach (Expression<Func<T, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            return  query.ToList();
        }

        public T GetById(Guid id) => db.Find<T>(id);

        public void Update(T item) =>
            db.Entry<T>(item).State = EntityState.Modified;
    }
}