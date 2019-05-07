using BankingApp.ModelsDTO;

namespace BankingApp.Services.Interface
{
    public interface IBankingService
    {
        double GetBankOperationMinAmoun();
        OperationDetails Deposit(BankOperation bankOperation);
        OperationDetails Withdraw(BankOperation bankOperation);
        OperationDetails Transfer(BankOperation bankOperation);
    }
}
