namespace BankingApp.ModelsDTO
{
    public class OperationDetails
    {
        public bool Succeeded { get; }
        public string Message { get; }
        public double Amount { get; }

        public static OperationDetails Error(string errorMessage) =>
            new OperationDetails(false, errorMessage,0);

        public static OperationDetails Success(double amount) =>
            new OperationDetails(true, "Success", amount);

        private OperationDetails(bool succeeded, string message, double amount)
        {
            Succeeded = succeeded;
            Message = message;
            Amount = amount;
        }
    }
}