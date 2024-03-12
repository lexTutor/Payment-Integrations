using Remita.Model.Common;
using System.Collections.Generic;

namespace Remita.Model.Request
{
    public class BulkTransactionInitiationRequest
    {
        public string BatchRef { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public string PayerName { get; set; }
        public string PayerEmail { get; set; }
        public string PayerPhone { get; set; }
        public string SourceNarration { get; set; }
        public string CustomReference { get; set; }
        public int GenerateRrrOnly { get; set; }
        public List<BulkPaymentTransactionData> Transactions { get; set; }
    }
}
