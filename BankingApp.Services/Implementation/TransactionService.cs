using BankingApp.ModelsDTO;
using System;
using System.Collections.Generic;
using BankingApp.Services.Interface;
using BankingApp.DataAccess.UowFactory;

namespace BankingApp.Services.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly IBankingUowFactory _bankingUow;

        public TransactionService(IBankingUowFactory uow) =>
            _bankingUow = uow;

        public IList<TransactionResult> GetByUser(Guid userId)
        {
            using (var bankingUow = _bankingUow.Create())
            {
                var transactions = bankingUow.Transaction.GatAllByUserId(userId);
                var listDTO = new List<TransactionResult>();

                foreach (var transaction in transactions)
                    listDTO.Add(TransactionResult.parsToDTO(transaction));

                return listDTO;
            }     
        }
    }
}