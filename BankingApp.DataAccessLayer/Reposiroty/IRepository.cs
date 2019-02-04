using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Reposiroty
{
    public interface IRepositoryBase<T> where T : class
    {
        IEnumerable<T> GetAll();
        void Create(T item);
        void Update(T item);
        Task<T> Get(Guid id);
    }

    public interface IRepositoryUser<T> : IRepositoryBase<T> where T : class {}

    public interface IRepositoryTransaction<T> : IRepositoryBase<T> where T : class{}
}
