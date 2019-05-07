using Microsoft.EntityFrameworkCore;
using BankingApp.ModelsDTO;
using BankingApp.Models;
using System;
using BankingApp.Services.Interface;
using BankingApp.DataAccess.UowFactory;

namespace BankingApp.Services.Implementation
{
    public class BankingService : IBankingService
    {
        private const int serverReconnections = 10;
        private const double _bankOperationMinAmount = 1;
        private readonly IBankingUowFactory _bankingUow;

        public BankingService(IBankingUowFactory uow) =>
            _bankingUow = uow;

        public double GetBankOperationMinAmoun() =>
            _bankOperationMinAmount;
        
        public OperationDetails Deposit(BankOperation bankOperation) =>
            BankOperation(bankOperation, Operation.Deposit);
        
        public OperationDetails Transfer(BankOperation bankOperation)
        {
            if(bankOperation == null || bankOperation.SenderId == bankOperation.RecipientId)
                return OperationDetails.Error("Recipient not found");

            return BankOperation(bankOperation, Operation.Transfer);
        }

        public OperationDetails Withdraw(BankOperation bankOperation) =>
            BankOperation(bankOperation, Operation.Withdraw);
        
        private OperationDetails BankOperation(BankOperation bankOperation, Operation operation)
        {
            OperationDetails details = TestDto(bankOperation);

            if (details != null)
                return details;

            for (int i = 0; i < serverReconnections; i++)
            {
                using (var bankingUOW = _bankingUow.Create())
                {
                    User userSender = bankingUOW.User.GetById(bankOperation.SenderId.Value);

                    if (operation != Operation.Deposit && userSender.Amount < bankOperation.Amount)
                        return OperationDetails.Error("There are not enough funds on the account");

                    var userRecipient = operation == Operation.Transfer ? bankingUOW.User.GetById(bankOperation.RecipientId) : null;

                    if (operation == Operation.Transfer && userRecipient == null)
                        return OperationDetails.Error("Recipient not found");

                    try
                    {
                        switch (operation)
                        {
                            case Operation.Deposit:

                                userSender.Amount += bankOperation.Amount;
                                break;

                            case Operation.Transfer:

                                userSender.Amount -= bankOperation.Amount;
                                userRecipient.Amount += bankOperation.Amount;
                                break;

                            case Operation.Withdraw:

                                userSender.Amount -= bankOperation.Amount;
                                break;
                        }

                        bankingUOW.Transaction.Create(new Transaction(bankOperation.SenderId.Value,
                            operation == Operation.Transfer ? bankOperation.RecipientId : bankOperation.SenderId.Value, bankOperation.Amount, operation));

                        bankingUOW.Save();
                        return OperationDetails.Success(userSender.Amount);
                    }
                    catch (DbUpdateConcurrencyException) {}
                }
            }

            return OperationDetails.Error("Server is busy");
        }

        private OperationDetails TestDto(BankOperation bankOperation)
        {
            if (bankOperation == null || bankOperation.SenderId == Guid.Empty)
                return OperationDetails.Error("User not found");

            if (bankOperation.Amount < _bankOperationMinAmount)
                return OperationDetails.Error("Invalid amount");

            return null;
        }
    }
}