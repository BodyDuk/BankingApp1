using Models;
using System;
using DataAccessLayer;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BankingApp.DataAccessLayer.Reposiroty
{
    public class TransactionRepository : IRepositoryTransaction<Transaction>
    {
        private BankContext db;

        public TransactionRepository(BankContext context)
        {
            this.db = context;
        }

        public void Create(Transaction item)
        {
            db.Transaction.Add(item);
        }

        public async Task<Transaction> Get(Guid id)
        {
            return await db.Transaction.FirstOrDefaultAsync(transactio => transactio.TransactionID == id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return db.Transaction;
        }

        public void Update(Transaction item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
