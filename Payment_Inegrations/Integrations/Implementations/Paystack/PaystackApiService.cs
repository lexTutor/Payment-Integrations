using Integrations.Interfaces;
using Integrations.Model.Common;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using PayStack.Net;

namespace Integrations.Implementations.Paystack
{
    public partial class PaystackApiService : IPaymentProvider
    {
        private readonly PayStackApi _payStackApi;
        private readonly IDistributedCache _distributedCache;

        public PaystackApiService(
            IDistributedCache distributedCache,
            IOptions<PayStackConfiguration> paystackConfigOptions)
        {
            _distributedCache = distributedCache;
            _payStackApi = new PayStackApi(paystackConfigOptions.Value.SecretKey);
        }

        public PaymentProvider PaymentProvider => PaymentProvider.PayStack;

    }
}
