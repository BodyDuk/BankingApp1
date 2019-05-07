using System;

namespace BankingApp.ModelsDTO
{
    public class BankOperation
    {
        public Guid? SenderId { get; set; }
        public Guid RecipientId { get; set; }
        public double Amount { get; set; }

        public BankOperation(Guid sender, Guid recipient, double amount)
        {
            SenderId = sender;
            RecipientId = recipient;
            Amount = amount;
        }

        public BankOperation(Guid sender, double amount)
        {
            SenderId = sender;
            RecipientId = sender;
            Amount = amount;
        }

        public BankOperation() { }
    }
}