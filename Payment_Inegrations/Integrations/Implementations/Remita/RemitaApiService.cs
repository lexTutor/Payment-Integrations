using Integrations.Interfaces;
using Integrations.Interfaces.Remita;
using Integrations.Model.Common;
using Microsoft.Extensions.Caching.Distributed;

namespace Integrations.Implementations.Remita
{
    public partial class RemitaApiService : IPaymentProvider
    {
        private readonly IRemitaHttpClient _httpClient;
        private readonly IDistributedCache _distributedCache;

        public RemitaApiService(IRemitaHttpClient httpClient, IDistributedCache distributedCache)
        {
            _httpClient = httpClient;
            _distributedCache = distributedCache;
        }

        public PaymentProvider PaymentProvider => PaymentProvider.Remita;
    }
}
