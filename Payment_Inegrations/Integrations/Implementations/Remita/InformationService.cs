using Integrations.Model.Api.Request;
using Integrations.Model.Api.Response;
using Integrations.Model.Common;
using Integrations.Utilities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integrations.Implementations.Remita
{
    public partial class RemitaApiService
    {
        public async Task<PaymentBaseResponse<IList<Bank>>> GetBanks(string fileName = null)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return PaymentBaseResponse<IList<Bank>>.Failed("Invalid Filename");

            var data = await _distributedCache.GetStringAsync(CacheConstants.RemitaBanks);

            if (!string.IsNullOrWhiteSpace(data))
                return PaymentBaseResponse<IList<Bank>>.Successful("Successful", JsonConvert.DeserializeObject<List<Bank>>(data));

            // Read the file contents
            string fileContents = File.ReadAllText(fileName);

            // Deserialize the file contents into a list of Bank objects
            var banksResponse = JsonConvert.DeserializeObject<List<Bank>>(fileContents);

            if (!banksResponse.IsNullOrEmpty())
            {
                await _distributedCache.SetStringAsync(CacheConstants.RemitaBanks, fileContents, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });

                return PaymentBaseResponse<IList<Bank>>.Successful("Successful", banksResponse);
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
