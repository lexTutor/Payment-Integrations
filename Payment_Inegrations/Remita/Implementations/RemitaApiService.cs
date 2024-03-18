using Integrations.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Integrations.Implementations
{
    public partial class RemitaApiService : IRemitaApiService
    {
        private readonly IRemitaHttpClient _httpClient;
        private readonly IDistributedCache _distributedCache;

        public RemitaApiService(IRemitaHttpClient httpClient, IDistributedCache distributedCache)
        {
            _httpClient = httpClient;
            _distributedCache = distributedCache;
        }
    }
}
