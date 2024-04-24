using Integrations.Model.Api.Request;
using Integrations.Model.Api.Response;
using Integrations.Model.Common;
using Integrations.Utilities;
using Integrations.Utilities.Serializers;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integrations.Implementations.Remita
{
    public partial class RemitaApiService
    {
        public async Task<PaymentBaseResponse<IList<Bank>>> GetBanks()
        {
            var data = await _distributedCache.GetStringAsync(CacheConstants.RemitaBanks);

            if (!string.IsNullOrWhiteSpace(data))
                return PaymentBaseResponse<IList<Bank>>.Successful("Successful", JsonSerializer.Deserialize<List<Bank>>(data));

            var banksResponse = await _httpClient.SendRequest<PaymentBaseResponse<BankResponse>>(HttpMethod.Get, EndpointConstants.Banks, nameof(GetBanks));

            if (banksResponse.IsSuccessful)
            {
                await _distributedCache.SetStringAsync(CacheConstants.RemitaBanks, JsonSerializer.ToJsonString(banksResponse.Data.Banks), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });

                return PaymentBaseResponse<IList<Bank>>.Successful("Successful", banksResponse.Data.Banks);
            }

            return PaymentBaseResponse<IList<Bank>>.Failed("Failed");
        }

        public async ValueTask<PaymentBaseResponse<AccountEnquiryResponse>> RetrieveAccountInformation(AccountEnquiryRequest accountEnquiryRequest, Func<object, string, string, string, Task> cleanUp = null)
        {
            if (string.IsNullOrWhiteSpace(accountEnquiryRequest.SourceAccount))
                return PaymentBaseResponse<AccountEnquiryResponse>.Failed("Failed", error: $"Invalid {nameof(accountEnquiryRequest.SourceAccount)}");

            if (string.IsNullOrWhiteSpace(accountEnquiryRequest.SourceBankCode))
                return PaymentBaseResponse<AccountEnquiryResponse>.Failed("Failed", error: $"Invalid {nameof(accountEnquiryRequest.SourceBankCode)}");

            var accountEnquiryResponse = await _httpClient.SendRequest<AccountEnquiryRequest, PaymentBaseResponse<AccountEnquiryResponse>>
                (HttpMethod.Post, EndpointConstants.AccountEnquiry, nameof(RetrieveAccountInformation), accountEnquiryRequest, cleanUp: cleanUp);

            return accountEnquiryResponse;
        }
    }
}
