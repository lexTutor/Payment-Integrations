using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Remita.Model.Request
{

    public class BulkTransactionInitiationRequest
    {
        [JsonPropertyName("batchRef")]
        public string BatchRef { get; set; }

        [JsonPropertyName("totalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("payerName")]
        public string PayerName { get; set; }

        [JsonPropertyName("payerEmail")]
        public string PayerEmail { get; set; }

        [JsonPropertyName("payerPhone")]
        public string PayerPhone { get; set; }

        [JsonPropertyName("sourceNarration")]
        public string SourceNarration { get; set; }

        [JsonPropertyName("customReference")]
        public string CustomReference { get; set; }

        [JsonPropertyName("generateRrrOnly")]
        public int GenerateRrrOnly { get; set; }

        [JsonPropertyName("transactions")]
        public List<BulkPaymentTransactionDataRequest> Transactions { get; set; }
    }

}
