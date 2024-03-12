namespace Remita.Model.Common
{
    public class BulkPaymentTransactionData
    {
        public decimal Amount { get; set; }
        public string TransactionRef { get; set; }
        public string DestinationAccount { get; set; }
        public string DestinationAccountName { get; set; }
        public string DestinationBankCode { get; set; }
        public string DestinationNarration { get; set; }
    }
}
