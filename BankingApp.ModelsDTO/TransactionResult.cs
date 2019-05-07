using BankingApp.Models;

namespace BankingApp.ModelsDTO
{
    public class TransactionResult
    {
        public string TimeStemp { get; set; }
        public string OperationName { get; set; }
        public double Amount { get; set; }
        public string SenderName { get; set; }
        public string RecipientName { get; set; }

        public static TransactionResult parsToDTO(Transaction transaction)
        {
            return new TransactionResult
            {
                TimeStemp = transaction.TimeStemp.ToShortDateString(),
                OperationName = transaction.OperationName.ToString(),
                Amount = transaction.Amount,
                SenderName = transaction.SenderUser?.Name,
                RecipientName = transaction.OperationName != Operation.Transfer ? "-" : transaction.RecipientUser?.Name
            };
        }
    }
}