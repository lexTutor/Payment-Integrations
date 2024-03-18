using System;

namespace Integrations.Model.Response
{
    public class TransactionDataResponse
    {
        public string TransactionRef { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionDescription { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Currency { get; set; }
        public string DestinationAccount { get; set; }
        public string DestinationBankCode { get; set; }
        public string SourceAccount { get; set; }
        public string SourceBankCode { get; set; }
    }
}
