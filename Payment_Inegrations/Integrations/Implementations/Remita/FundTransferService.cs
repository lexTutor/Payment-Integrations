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
        public async Task<PaymentBaseResponse<BulkTransactionStatusResponse>> BulkTransactionStatusEnquiry
            (string transactionRef, int page = 1, int pageSize = 50, Func<object, string, string, string, Task> cleanUp = null)
        {
            if (string.IsNullOrWhiteSpace(transactionRef))
                return PaymentBaseResponse<BulkTransactionStatusResponse>.Failed("Failed", error: "Invalid TransactionRef");

            return await _httpClient.SendRequest<PaymentBaseResponse<BulkTransactionStatusResponse>>
                (HttpMethod.Get, string.Format(EndpointConstants.BulkPaymentStatusEnquiry, transactionRef), nameof(BulkTransactionStatusEnquiry), cleanUp);
        }

        public async Task<PaymentBaseResponse<SingleTransactionInitiationResponse>> InitiateTransaction(SingleTransactionInitiationRequest singleTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null)
            => await _httpClient.SendRequest<SingleTransactionInitiationRequest, PaymentBaseResponse<SingleTransactionInitiationResponse>>
                (HttpMethod.Post, EndpointConstants.InitiateSingleTransaction, nameof(InitiateTransaction), singleTransactionInitiationRequest, cleanUp);

        public async Task<PaymentBaseResponse<BulkTransactionInitiationResponse>> InitiateTransaction(BulkTransactionInitiationRequest bulkTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null)
            => await _httpClient.SendRequest<BulkTransactionInitiationRequest, PaymentBaseResponse<BulkTransactionInitiationResponse>>
                (HttpMethod.Post, EndpointConstants.InitiateBulkTransaction, nameof(InitiateTransaction), bulkTransactionInitiationRequest, cleanUp);

        public async Task<PaymentBaseResponse<TransactionDataResponse>> SingleTransactionStatusEnquiry(string transactionRef,
            Func<object, string, string, string, Task> cleanUp = null)
        {
            if (string.IsNullOrWhiteSpace(transactionRef))
                return PaymentBaseResponse<TransactionDataResponse>.Failed("Failed", error: "Invalid TransactionRef");

            return await _httpClient.SendRequest<PaymentBaseResponse<TransactionDataResponse>>
                (HttpMethod.Get, string.Format(EndpointConstants.SinglePaymentStatusEnquiry, transactionRef), nameof(SingleTransactionStatusEnquiry), cleanUp);
        }
    }
}
