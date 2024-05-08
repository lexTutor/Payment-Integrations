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

        [JsonPropertyName("lineitems")]
        public List<BulkSettleTransactionDataRequest> LineItems { get; set; }
    }

    public class BulkSettleTransactionDataRequest
    {
        [JsonPropertyName("lineItemsId")]
        public string LineItemsId { get; set; }

        [JsonPropertyName("deductFeeFrom")]
        public string DeductFeeFrom { get; set; }

        [JsonPropertyName("beneficiaryAmount")]
        public string BeneficiaryAmount { get; set; }

        [JsonPropertyName("beneficiaryName")]
        public string BeneficiaryName { get; set; }

        [JsonPropertyName("bankCode")]
        public string BankCode { get; set; }

        [JsonPropertyName("beneficiaryAccount")]
        public string BeneficiaryAccount { get; set; }
    }
}
