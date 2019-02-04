using BankingApp.DataAccessLayer.Reposiroty;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingApp.DataAccessLayer.UOW
{
    public class BankingUOW : IDisposable
    {
        private BankContext db = new BankContext();
        //private IRepositoryUser userRepository;

        private UserRepositiry userRepository;
        private TransactionRepository transactionRepository;

        public UserRepositiry User
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepositiry(db);
                return userRepository;
            }
        }

        public TransactionRepository Transaction
        {
            get
            {
                if (transactionRepository == null)
                    transactionRepository = new TransactionRepository(db);
                return transactionRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
