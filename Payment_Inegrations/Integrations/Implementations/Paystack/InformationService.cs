using Integrations.Model.Api.Request;
using Integrations.Model.Api.Response;
using Integrations.Model.Common;
using Integrations.Utilities;
using Integrations.Utilities.Serializers;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integrations.Implementations.Paystack
{
    public partial class PaystackApiService
    {
        public async Task<PaymentBaseResponse<IList<Bank>>> GetBanks(string fileName = null)
        {
            var data = await _distributedCache.GetStringAsync(CacheConstants.PaystackBanks);

            if (!string.IsNullOrWhiteSpace(data))
                return PaymentBaseResponse<IList<Bank>>.Successful("Successful", JsonSerializer.Deserialize<List<Bank>>(data));

            var banksResponse = _payStackApi.Miscellaneous.ListBanks(1000);

            if (banksResponse.Status)
            {
                var banks = banksResponse.Data.Select(b => new Bank
                {
                    BankAccronym = b.Slug,
                    BankName = b.Name,
                    BankCode = b.Code
                }).ToList();

                await _distributedCache.SetStringAsync(CacheConstants.RemitaBanks, JsonSerializer.ToJsonString(banks), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                });

                return PaymentBaseResponse<IList<Bank>>.Successful("Successful", banks);
            }

            return PaymentBaseResponse<IList<Bank>>.Failed("Failed");
        }

        public async ValueTask<PaymentBaseResponse<AccountEnquiryResponse>> RetrieveAccountInformation
            (AccountEnquiryRequest accountEnquiryRequest, Func<object, string, string, string, Task> cleanUp = null)
        {
            if (string.IsNullOrWhiteSpace(accountEnquiryRequest.SourceAccount))
                return PaymentBaseResponse<AccountEnquiryResponse>.Failed("Failed", error: $"Invalid {nameof(accountEnquiryRequest.SourceAccount)}");

            if (string.IsNullOrWhiteSpace(accountEnquiryRequest.SourceBankCode))
                return PaymentBaseResponse<AccountEnquiryResponse>.Failed("Failed", error: $"Invalid {nameof(accountEnquiryRequest.SourceBankCode)}");

            var response = _payStackApi.Miscellaneous.ResolveAccountNumber(accountEnquiryRequest.SourceAccount, accountEnquiryRequest.SourceBankCode);

            if (cleanUp != null)
            {
                await cleanUp(accountEnquiryRequest, response.RawJson, HttpMethod.Get.ToString(), nameof(_payStackApi.Transfers.ListTransfers));
            }

            if (response?.Data == null || !response.Status)
                return PaymentBaseResponse<AccountEnquiryResponse>.Failed("Failed", error: response?.Message ?? "Account information not found");

            return PaymentBaseResponse<AccountEnquiryResponse>.Successful("Successful", new AccountEnquiryResponse
            {
                SourceBankCode = accountEnquiryRequest.SourceBankCode,
                SourceAccount = response.Data.AccountNumber,
                SourceAccountName = response.Data.AccountName
            });
        }
    }
}
