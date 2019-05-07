using BankingApp.Models;
using System;
using System.Collections.Generic;

namespace BankingApp.DataAccess.Reposiroty
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        IList<Transaction> GatAllByUserId(Guid userId);
    }

    public class TransactionRepository :  RepositoryBase<Transaction>,ITransactionRepository
    {
        public TransactionRepository(BankContext context) : base(context)  {}

        public IList<Transaction> GatAllByUserId(Guid userId) =>
            base.Get((a => a.RecipientId == userId || a.SenderId == userId), null, (a => a.RecipientUser), (a => a.SenderUser));
    }
}