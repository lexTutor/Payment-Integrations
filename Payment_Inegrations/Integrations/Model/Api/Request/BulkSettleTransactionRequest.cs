using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Integrations.Model.Api.Request
{
    public class BulkSettleTransactionRequest
    {
        [JsonPropertyName("serviceTypeId")]
        public string ServiceTypeId { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }

        [JsonPropertyName("payerName")]
        public string PayerName { get; set; }

        [JsonPropertyName("payerEmail")]
        public string PayerEmail { get; set; }

        [JsonPropertyName("payerPhone")]
        public string PayerPhone { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("LINEITEMS")]
        public List<BulkSettleTransactionDataRequest> LineItems { get; set; }
    }

    public class BulkSettleTransactionDataRequest
    {
        [JsonPropertyName("LINEITEMSID")]
        public string LineItemsId { get; set; }

        [JsonPropertyName("DEDUCTFEEFROM")]
        public string DeductFeeFrom { get; set; }

        [JsonPropertyName("BENEFICIARYAMOUNT")]
        public string BeneficiaryAmount { get; set; }

        [JsonPropertyName("BENEFICIARYNAME")]
        public string BeneficiaryName { get; set; }

        [JsonPropertyName("BANKCODE")]
        public string bankCode { get; set; }

        [JsonPropertyName("BENEFICIARYACCOUNT")]
        public string beneficiaryAccount { get; set; }
    }
}
