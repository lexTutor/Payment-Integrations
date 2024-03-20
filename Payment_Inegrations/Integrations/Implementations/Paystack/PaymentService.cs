using Integrations.Model.ApiModels.Request;
using Integrations.Model.Common;
using PayStack.Net;
using System.Threading.Tasks;

namespace Integrations.Implementations.Paystack
{
    public partial class PaystackApiService
    {
        public Task<PaymentBaseResponse<string>> GeneratePaymentUrl(GeneratePaymentUrl generatePaymentUrl)
        {
            var response = _payStackApi.Transactions.Initialize(new TransactionInitializeRequest
            {
                AmountInKobo = generatePaymentUrl.Amount * 100,
                CallbackUrl = generatePaymentUrl.CallbackUrl,
                Currency = generatePaymentUrl.Currency,
                Email = generatePaymentUrl.Email,
                Reference = generatePaymentUrl.TransactionReference
            });

            if (response?.Data == null || response.Status)
            {
                return Task.FromResult(PaymentBaseResponse<string>.Successful(response.Message, response.Data.AuthorizationUrl));
            }
            else
            {
                return Task.FromResult(PaymentBaseResponse<string>.Failed(response.Message));
            }
        }
    }
}
