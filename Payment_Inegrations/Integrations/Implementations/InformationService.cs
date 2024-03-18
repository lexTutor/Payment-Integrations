using Integrations.Model.Request;
using Integrations.Model.Response;
using Integrations.Utilities;
using Integrations.Utilities.Serializers;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integrations.Implementations
{
    public partial class RemitaApiService
    {
        public async Task<RemitaBaseResponse<IList<Bank>>> GetBanks()
        {
            var data = await _distributedCache.GetStringAsync(CacheConstants.Banks);

            if (!string.IsNullOrWhiteSpace(data))
                return RemitaBaseResponse<IList<Bank>>.Successful("Successful", JsonSerializer.Deserialize<List<Bank>>(data));

            var banksResponse = await _httpClient.SendRequest<RemitaBaseResponse<BankResponse>>(HttpMethod.Get, EndpointConstants.Banks, nameof(GetBanks));

            if (banksResponse.IsSuccessful)
            {
                await _distributedCache.SetStringAsync(CacheConstants.Banks, JsonSerializer.ToJsonString(banksResponse.Data.Banks), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });

                return RemitaBaseResponse<IList<Bank>>.Successful("Successful", banksResponse.Data.Banks);
            }

            return RemitaBaseResponse<IList<Bank>>.Failed("Failed");
        }

        public async Task<RemitaBaseResponse<AccountEnquiryResponse>> RetrieveAccountInformation(AccountEnquiryRequest accountEnquiryRequest, Func<object, string, string, string, Task> cleanUp = null)
        {
            if (string.IsNullOrWhiteSpace(accountEnquiryRequest.SourceAccount))
                return RemitaBaseResponse<AccountEnquiryResponse>.Failed("Failed", error: $"Invalid {nameof(accountEnquiryRequest.SourceAccount)}");

            if (string.IsNullOrWhiteSpace(accountEnquiryRequest.SourceBankCode))
                return RemitaBaseResponse<AccountEnquiryResponse>.Failed("Failed", error: $"Invalid {nameof(accountEnquiryRequest.SourceBankCode)}");

            var accountEnquiryResponse = await _httpClient.SendRequest<AccountEnquiryRequest, RemitaBaseResponse<AccountEnquiryResponse>>
                (HttpMethod.Post, EndpointConstants.AccountEnquiry, nameof(RetrieveAccountInformation), accountEnquiryRequest, cleanUp);

            return accountEnquiryResponse;
        }
    }
}
