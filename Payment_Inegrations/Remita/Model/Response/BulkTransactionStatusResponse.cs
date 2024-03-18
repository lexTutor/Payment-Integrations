using System;
using System.Collections.Generic;

namespace Integrations.Model.Response
{
    public class BulkTransactionStatusResponse
    {
        public string BatchRef { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FeeAmount { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string Currency { get; set; }
        public string PaymentStatus { get; set; }
        public List<BulkPaymentTransactionDataResponse> Transactions { get; set; }
    }

    public class BulkPaymentTransactionDataResponse
    {
        public decimal Amount { get; set; }
        public string TransactionRef { get; set; }
        public string DestinationAccount { get; set; }
        public string DestinationBankCode { get; set; }
        public string DestinationNarration { get; set; }
        public string PaymentStatus { get; set; }
        public string StatusMessage { get; set; }
    }
}
