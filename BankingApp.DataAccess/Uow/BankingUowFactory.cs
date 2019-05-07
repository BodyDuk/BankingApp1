using BankingApp.DataAccess.Uow;

namespace BankingApp.DataAccess.UowFactory
{
    public  interface IBankingUowFactory
    {
        IBankingUow Create();
    }

    public class BankingUowFactory : IBankingUowFactory
    {
        public IBankingUow Create() => new BankingUow();
    }
}