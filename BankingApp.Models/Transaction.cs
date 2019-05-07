using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApp.Models
{
    public enum Operation
    {
        Deposit,
        Withdraw,
        Transfer
    }

    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TransactionId { get; set; }
        public DateTime TimeStemp { get; set; }
        public Operation OperationName { get; set; }
        public double Amount { get; set; }

        public virtual User SenderUser { get; set; }
        public virtual User RecipientUser { get; set; }

        public Guid SenderId { get; set; }
        public Guid RecipientId { get; set; }

        public Transaction(Guid senderId, double smount, Operation operation)
        {
            TimeStemp = DateTime.Now;
            SenderId = senderId;
            RecipientId = SenderId;
            Amount = smount;
            OperationName = operation;
        }

        public  Transaction(Guid senderId, Guid recipientId, double amount, Operation operation)
        {
            TimeStemp = DateTime.Now;
            SenderId = senderId;
            RecipientId = recipientId;
            Amount = amount;
            OperationName = operation;
        }

        public Transaction() { }
    }
}