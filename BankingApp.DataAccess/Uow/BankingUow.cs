using BankingApp.DataAccess.Reposiroty;
using System;

namespace BankingApp.DataAccess.Uow
{
    public interface IBankingUow : IDisposable
    {
        void Save();
        IUserRepositiry User { get; }
        ITransactionRepository Transaction { get; }
    }

    public class BankingUow : IBankingUow
    {
        private bool disposed = false;
        private BankContext db = new BankContext();
        private IUserRepositiry _userRepository;
        private ITransactionRepository _transactionRepository;

        public IUserRepositiry User =>
            _userRepository == null ? _userRepository = new UserRepositiry(db) : _userRepository;

        public ITransactionRepository Transaction =>
            _transactionRepository == null ? _transactionRepository = new TransactionRepository(db) : _transactionRepository;

        public void Save() => db.SaveChanges();

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}