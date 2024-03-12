using System;

namespace Remita.Model.Response
{
    public class SingleTransactionInitiationResponse
    {
        public decimal Amount { get; set; }
        public string TransactionRef { get; set; }
        public string TransactionDescription { get; set; }
        public string AuthorizationId { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
