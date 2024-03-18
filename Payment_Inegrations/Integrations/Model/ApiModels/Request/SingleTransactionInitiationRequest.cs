using Integrations.Utilities;
using System.Text.Json.Serialization;

namespace Integrations.Model.Api.Request
{
    public class SingleTransactionInitiationRequest
    {
        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("transactionRef")]
        public string TransactionRef { get; set; }

        [JsonPropertyName("transactionDescription")]
        public string TransactionDescription { get; set; }

        [JsonPropertyName("channel")]
        public string Channel => GenericConstants.WebChannel;

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("destinationAccount")]
        public string DestinationAccount { get; set; }

        [JsonPropertyName("destinationAccountName")]
        public string DestinationAccountName { get; set; }

        [JsonPropertyName("generateRrrOnly")]
        public int GenerateRrrOnly => GenerateRrrOnlyBool ? 1 : 0;

        [JsonIgnore]
        public bool GenerateRrrOnlyBool { get; set; }

        [JsonPropertyName("destinationBankCode")]
        public string DestinationBankCode { get; set; }

        [JsonPropertyName("destinationEmail")]
        public string DestinationEmail { get; set; }

        [JsonPropertyName("sourceAccount")]
        public string SourceAccount { get; set; }

        [JsonPropertyName("sourceAccountName")]
        public string SourceAccountName { get; set; }

        [JsonPropertyName("sourceBankCode")]
        public string SourceBankCode { get; set; }

        [JsonPropertyName("originalAccountNumber")]
        public string OriginalAccountNumber { get; set; }

        [JsonPropertyName("originalBankCode")]
        public string OriginalBankCode { get; set; }

        [JsonPropertyName("customReference")]
        public string CustomReference { get; set; }
    }

}
