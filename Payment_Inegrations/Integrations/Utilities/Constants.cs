namespace Integrations.Utilities
{
    public class PaymentStatusConstants
    {
        public const string Pending = "PENDING";
        public const string PendingDebit = "PENDING_DEBIT";
    }

    public class DemoCredentialConstants
    {
        public const string UserName = "GY7OMET977KCBC7Y";
        public const string Password = "ZDBMX205PUOLLKY2Q4U395FNL384JVW4";
    }

    public class EndpointConstants
    {
        public const string AuthUrl = "uaasvc/uaa/token";
        public const string Banks = "rpgsvc/v3/rpg/banks";
        public const string InitiateSingleTransaction = "rpgsvc/v3/rpg/single/payment";
        public const string InitiateBulkTransaction = "rpgsvc/v3/rpg/bulk/payment";
        public const string AccountEnquiry = "rpgsvc/v3/rpg/account/lookup";
        public const string BulkPaymentStatusEnquiry = "rpgsvc/v3/rpg/bulk/payment/status/{0}";
        public const string SinglePaymentStatusEnquiry = "rpgsvc/v3/rpg/single/payment/status/{0}";
    }

    public class GenericConstants
    {
        public const string WebChannel = "WEB";
        public const string RemitaHttpClientName = "Remita";
    }

    public class CacheConstants
    {
        public const string RemitaBanks = "RemitaBanks";
        public const string PaystackBanks = "PaystackBanks";
        public const string AccessToken = "remitaaccesstoken";
    }
}
