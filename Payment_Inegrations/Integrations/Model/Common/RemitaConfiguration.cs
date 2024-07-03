namespace Integrations.Model.Common
{
    public class RemitaConfiguration
    {
        public const string ConfigKey = "Remita";
        public string ApiKey { get; set; }
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public string SettlementServiceTypeId { get; set; }
    }
}
