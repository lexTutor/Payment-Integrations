using Integrations.Model.Api.Request;
using Integrations.Model.Api.Response;
using Integrations.Model.Common;
using Integrations.Utilities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integrations.Implementations.Remita
{
    public partial class RemitaApiService
    {
        public async ValueTask<PaymentBaseResponse<BulkTransactionStatusResponse>> BulkTransactionStatusEnquiry
            (string transactionRef, int page = 1, int pageSize = 50, Func<object, string, string, string, Task> cleanUp = null)
        {
            if (string.IsNullOrWhiteSpace(transactionRef))
                return PaymentBaseResponse<BulkTransactionStatusResponse>.Failed("Failed", error: "Invalid TransactionRef");

            return await _httpClient.SendRequest<PaymentBaseResponse<BulkTransactionStatusResponse>>
                (HttpMethod.Get, string.Format(EndpointConstants.BulkPaymentStatusEnquiry, transactionRef), nameof(BulkTransactionStatusEnquiry), cleanUp: cleanUp);
        }

        public async ValueTask<PaymentBaseResponse<SingleTransactionInitiationResponse>> InitiateTransaction(SingleTransactionInitiationRequest singleTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null)
            => await _httpClient.SendRequest<SingleTransactionInitiationRequest, PaymentBaseResponse<SingleTransactionInitiationResponse>>
                (HttpMethod.Post, EndpointConstants.InitiateSingleTransaction, nameof(InitiateTransaction), singleTransactionInitiationRequest, cleanUp: cleanUp);

        public async ValueTask<PaymentBaseResponse<BulkTransactionInitiationResponse>> InitiateTransaction(BulkTransactionInitiationRequest bulkTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null)
            => await _httpClient.SendRequest<BulkTransactionInitiationRequest, PaymentBaseResponse<BulkTransactionInitiationResponse>>
                (HttpMethod.Post, EndpointConstants.InitiateBulkTransaction, nameof(InitiateTransaction), bulkTransactionInitiationRequest, cleanUp: cleanUp);

        public async ValueTask<PaymentBaseResponse<TransactionDataResponse>> SingleTransactionStatusEnquiry(string transactionRef,
            Func<object, string, string, string, Task> cleanUp = null)
        {
            if (string.IsNullOrWhiteSpace(transactionRef))
                return PaymentBaseResponse<TransactionDataResponse>.Failed("Failed", error: "Invalid TransactionRef");

            return await _httpClient.SendRequest<PaymentBaseResponse<TransactionDataResponse>>
                (HttpMethod.Get, string.Format(EndpointConstants.SinglePaymentStatusEnquiry, transactionRef), nameof(SingleTransactionStatusEnquiry), cleanUp: cleanUp);
        }

        public async Task<PaymentBaseResponse<SettleTransactionResponse>> SettleBulkTransaction(BulkSettleTransactionRequest settleTransactionRequest, Func<object, string, string, string, Task> cleanUp = null)
        {
            var hash = AppHelpers.ComputeSha512Hash(_configuration.MerchantId + settleTransactionRequest.ServiceTypeId + settleTransactionRequest.OrderId
                + settleTransactionRequest.Amount + _configuration.ApiKey);

            var hashToken = $"remitaConsumerKey={_configuration.MerchantId},remitaConsumerToken={hash}";

            var apiResponse = await _httpClient.SendRequest<BulkSettleTransactionRequest, SettleTransactionResponse>
                (HttpMethod.Post, EndpointConstants.InitiateBulkSettlement, nameof(SettleBulkTransaction),
                settleTransactionRequest, hashToken: hashToken, cleanUp: cleanUp);

            if (apiResponse.IsSuccessful)
                return PaymentBaseResponse<SettleTransactionResponse>.Successful("Successful", apiResponse);

            return PaymentBaseResponse<SettleTransactionResponse>.Failed("Failed", apiResponse);
        }

        public async Task<PaymentBaseResponse<SettleBulkTransactionStatusResponse>> SettleBulkTransactionEnquiry(string rrr, Func<object, string, string, string, Task> cleanUp = null)
        {
            var hashToken = AppHelpers.ComputeSha512Hash(rrr + _configuration.ApiKey + _configuration.MerchantId);

            var apiResponse = await _httpClient.SendRequest<SettleBulkTransactionStatusResponse>
               (HttpMethod.Get, string.Format(EndpointConstants.BulkSettlementStatus, _configuration.MerchantId, rrr, hashToken),
               nameof(SettleBulkTransactionEnquiry), hashToken: hashToken, cleanUp: cleanUp);

            if (apiResponse.IsSuccessful)
                return PaymentBaseResponse<SettleBulkTransactionStatusResponse>.Successful("Successful", apiResponse);

            return PaymentBaseResponse<SettleBulkTransactionStatusResponse>.Failed("Failed", apiResponse);
        }
    }
}
