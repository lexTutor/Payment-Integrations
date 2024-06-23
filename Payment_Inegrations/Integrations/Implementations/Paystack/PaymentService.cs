using Integrations.Model.Api.Request;
using Integrations.Model.Common;
using PayStack.Net;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integrations.Implementations.Paystack
{
    public partial class PaystackApiService
    {
        public async ValueTask<PaymentBaseResponse<string>> GeneratePaymentUrl(GeneratePaymentUrl paymentData, Func<object, string, string, string, Task> cleanUp = null)
        {
            var response = _payStackApi.Transactions.Initialize(new TransactionInitializeRequest
            {
                AmountInKobo = (int)Math.Ceiling(paymentData.Amount * 100),
                CallbackUrl = paymentData.CallbackUrl,
                Currency = paymentData.Currency,
                Email = paymentData.Email,
                Reference = paymentData.TransactionReference
            });

            if (cleanUp != null)
            {
                await cleanUp(paymentData, response.RawJson, HttpMethod.Post.ToString(), nameof(_payStackApi.Transactions));
            }

            if (response.Status && response.Data != null)
            {
                return PaymentBaseResponse<string>.Successful(response.Message, response.Data.AuthorizationUrl);
            }
            else
            {
                return PaymentBaseResponse<string>.Failed(response.Message, "Unable to initiate transaction.");
            }
        }
    }
}
