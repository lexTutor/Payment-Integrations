using System.Collections.Generic;
using System;
using Remita.Model.Common;

namespace Remita.Model.Response
{
    public class BulkTransactionStatusResponse
    {
        public string BatchRef { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FeeAmount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Currency { get; set; }
        public string PaymentStatus { get; set; }
        public List<BulkPaymentTransactionData> Transactions { get; set; }
    }
}
