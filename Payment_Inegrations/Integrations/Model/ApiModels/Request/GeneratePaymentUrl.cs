﻿using Newtonsoft.Json;

namespace Integrations.Model.ApiModels.Request
{
    public class GeneratePaymentUrl
    {
        [JsonProperty("amount")]
        public string TransactionReference { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("callback_url")]
        public string CallbackUrl { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}
