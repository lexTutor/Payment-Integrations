using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Integrations.Model.Api.Request
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
        public int GenerateRrrOnly => GenerateRrrOnlyBool ? 1 : 0;

        public bool GenerateRrrOnlyBool { get; set; }

        [JsonPropertyName("transactions")]
        public List<BulkPaymentTransactionDataRequest> Transactions { get; set; }
    }

}
