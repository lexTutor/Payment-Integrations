using System;

namespace Integrations.Model.Api.Response
{
    public class SingleTransactionInitiationResponse
    {
        public string AuthorizationUrl { get; set; }
        public decimal Amount { get; set; }
        public string TransactionRef { get; set; }
        public string TransactionDescription { get; set; }
        public string AuthorizationId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
