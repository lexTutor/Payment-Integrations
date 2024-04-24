using Integrations.Interfaces;
using Integrations.Interfaces.Remita;
using Integrations.Model.Common;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Integrations.Implementations.Remita
{
    public partial class RemitaApiService : IPaymentProvider
    {
        private readonly IRemitaHttpClient _httpClient;
        private readonly IDistributedCache _distributedCache;
        private readonly RemitaConfiguration _configuration;

        public RemitaApiService(IRemitaHttpClient httpClient, IDistributedCache distributedCache, IOptions<RemitaConfiguration> configuration)
        {
            _httpClient = httpClient;
            _distributedCache = distributedCache;
            _configuration = configuration.Value;
        }

        public PaymentProvider PaymentProvider => PaymentProvider.Remita;
    }
}
