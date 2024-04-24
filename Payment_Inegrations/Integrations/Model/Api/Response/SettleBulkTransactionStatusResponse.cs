using Integrations.Model.Common;

namespace Integrations.Model.Api.Response
{
    public class SettleBulkTransactionStatusResponse : ErrorResponse
    {
        public double Amount { get; set; }
        public string RRR { get; set; }
        public string OrderId { get; set; }
        public string Message { get; set; }
        public string TransactionTime { get; set; }
        public string Status { get; set; }
    }
}
