using Integrations.Model.Request;
using Integrations.Model.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integrations.Interfaces
{
    public interface IRemitaApiService
    {
        Task<RemitaBaseResponse<IList<Bank>>> GetBanks();
        Task<RemitaBaseResponse<AccountEnquiryResponse>> RetrieveAccountInformation(AccountEnquiryRequest accountEnquiryRequest, Func<object, string, string, string, Task> cleanUp = null);
        Task<RemitaBaseResponse<SingleTransactionInitiationResponse>> InitiateTransaction(SingleTransactionInitiationRequest singleTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null);
        Task<RemitaBaseResponse<BulkTransactionInitiationResponse>> InitiateTransaction(BulkTransactionInitiationRequest bulkTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null);
        Task<RemitaBaseResponse<TransactionDataResponse>> SingleTransactionStatusEnquiry(string transactionRef, Func<object, string, string, string, Task> cleanUp = null);
        Task<RemitaBaseResponse<BulkTransactionStatusResponse>> BulkTransactionStatusEnquiry(string transactionRef, Func<object, string, string, string, Task> cleanUp = null);
    }
}
