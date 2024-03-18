using System.Text.Json.Serialization;

namespace Integrations.Model.Request
{
    public class AccountEnquiryRequest
    {
        [JsonPropertyName("sourceAccount")]
        public string SourceAccount { get; set; }
        [JsonPropertyName("sourceBankCode")]
        public string SourceBankCode { get; set; }
    }
}
