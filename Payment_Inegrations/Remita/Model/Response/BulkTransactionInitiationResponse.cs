using System;

namespace Remita.Model.Response
{
    public class BulkTransactionInitiationResponse
    {
        public string BatchRef { get; set; }
        public decimal TotalAmount { get; set; }
        public string AuthorizationId { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
