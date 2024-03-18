using Integrations.Model.Request;
using Integrations.Model.Response;
using Integrations.Utilities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integrations.Implementations
{
    public partial class RemitaApiService
    {
        public async Task<RemitaBaseResponse<BulkTransactionStatusResponse>> BulkTransactionStatusEnquiry(string transactionRef,
            Func<object, string, string, string, Task> cleanUp = null)
        {
            if (string.IsNullOrWhiteSpace(transactionRef))
                return RemitaBaseResponse<BulkTransactionStatusResponse>.Failed("Failed", error: "Invalid TransactionRef");

            return await _httpClient.SendRequest<RemitaBaseResponse<BulkTransactionStatusResponse>>
                (HttpMethod.Get, string.Format(EndpointConstants.BulkPaymentStatusEnquiry, transactionRef), nameof(BulkTransactionStatusEnquiry), cleanUp);
        }

        public async Task<RemitaBaseResponse<SingleTransactionInitiationResponse>> InitiateTransaction(SingleTransactionInitiationRequest singleTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null)
            => await _httpClient.SendRequest<SingleTransactionInitiationRequest, RemitaBaseResponse<SingleTransactionInitiationResponse>>
                (HttpMethod.Post, EndpointConstants.InitiateSingleTransaction, nameof(InitiateTransaction), singleTransactionInitiationRequest, cleanUp);

        public async Task<RemitaBaseResponse<BulkTransactionInitiationResponse>> InitiateTransaction(BulkTransactionInitiationRequest bulkTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null)
            => await _httpClient.SendRequest<BulkTransactionInitiationRequest, RemitaBaseResponse<BulkTransactionInitiationResponse>>
                (HttpMethod.Post, EndpointConstants.InitiateBulkTransaction, nameof(InitiateTransaction), bulkTransactionInitiationRequest, cleanUp);

        public async Task<RemitaBaseResponse<TransactionDataResponse>> SingleTransactionStatusEnquiry(string transactionRef,
            Func<object, string, string, string, Task> cleanUp = null)
        {
            if (string.IsNullOrWhiteSpace(transactionRef))
                return RemitaBaseResponse<TransactionDataResponse>.Failed("Failed", error: "Invalid TransactionRef");

            return await _httpClient.SendRequest<RemitaBaseResponse<TransactionDataResponse>>
                (HttpMethod.Get, string.Format(EndpointConstants.SinglePaymentStatusEnquiry, transactionRef), nameof(SingleTransactionStatusEnquiry), cleanUp);
        }
    }
}
