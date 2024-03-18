using System.Text.Json.Serialization;

namespace Integrations.Model.Request
{
    public class BulkPaymentTransactionDataRequest
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
        [JsonPropertyName("transactionRef")]
        public string TransactionRef { get; set; }
        [JsonPropertyName("destinationAccount")]
        public string DestinationAccount { get; set; }
        [JsonPropertyName("destinationAccountName")]
        public string DestinationAccountName { get; set; }
        [JsonPropertyName("destinationBankCode")]
        public string DestinationBankCode { get; set; }
        [JsonPropertyName("destinationNarration")]
        public string DestinationNarration { get; set; }
    }
}
