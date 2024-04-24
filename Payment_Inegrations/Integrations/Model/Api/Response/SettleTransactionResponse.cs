using Integrations.Model.Common;
using System.Text.Json.Serialization;

namespace Integrations.Model.Api.Response
{
    public class SettleTransactionResponse : ErrorResponse
    {
        [JsonPropertyName("statuscode")]
        public string StatusCode { get; set; }

        [JsonPropertyName("RRR")]
        public string RRR { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
