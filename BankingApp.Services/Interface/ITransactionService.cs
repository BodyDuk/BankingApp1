using BankingApp.ModelsDTO;
using System;
using System.Collections.Generic;

namespace BankingApp.Services.Interface
{
    public interface ITransactionService
    {
        IList<TransactionResult> GetByUser(Guid userId);
    }
}