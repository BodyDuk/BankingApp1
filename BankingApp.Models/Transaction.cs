using System;

namespace Models
{
    public class Transaction
    {
        public Guid TransactionID { get; set; }

        public DateTime TimeStemp { get; set; }

        public string OperationName { get; set; }

        public Int64 Amount { get; set; }

        public User SenderUser { get; set; }

        public Guid SenderID { get; set; }

        public Guid RecipientID { get; set; }
    }
}