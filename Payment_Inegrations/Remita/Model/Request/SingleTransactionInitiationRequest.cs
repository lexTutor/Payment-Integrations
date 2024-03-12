using Remita.Utilities;

namespace Remita.Model.Request
{
    public class SingleTransactionInitiationRequest
    {
        public decimal Amount { get; set; }
        public string TransactionRef { get; set; }
        public string TransactionDescription { get; set; }
        public string Channel => GenericConstants.WebChannel;
        public string Currency { get; set; }
        public string DestinationAccount { get; set; }
        public string DestinationAccountName { get; set; }
        public bool GenerateRrrOnlyBool { get; set; }
        public int GenerateRrrOnly => GenerateRrrOnlyBool ? 1 : 0;
        public string DestinationBankCode { get; set; }
        public string DestinationEmail { get; set; }
        public string SourceAccount { get; set; }
        public string SourceAccountName { get; set; }
        public string SourceBankCode { get; set; }
        public string OriginalAccountNumber { get; set; }
        public string OriginalBankCode { get; set; }
        public string CustomReference { get; set; }
    }
}
