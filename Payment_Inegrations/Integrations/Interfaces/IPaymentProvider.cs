using Integrations.Model.Api.Request;
using Integrations.Model.Api.Response;
using Integrations.Model.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integrations.Interfaces
{
    public interface IPaymentProvider
    {
        PaymentProvider PaymentProvider { get; }
        Task<PaymentBaseResponse<IList<Bank>>> GetBanks();
        Task<PaymentBaseResponse<AccountEnquiryResponse>> RetrieveAccountInformation(AccountEnquiryRequest accountEnquiryRequest, Func<object, string, string, string, Task> cleanUp = null);
        Task<PaymentBaseResponse<SingleTransactionInitiationResponse>> InitiateTransaction(SingleTransactionInitiationRequest singleTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null);
        Task<PaymentBaseResponse<BulkTransactionInitiationResponse>> InitiateTransaction(BulkTransactionInitiationRequest bulkTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null);
        Task<PaymentBaseResponse<TransactionDataResponse>> SingleTransactionStatusEnquiry(string transactionRef, Func<object, string, string, string, Task> cleanUp = null);
        Task<PaymentBaseResponse<BulkTransactionStatusResponse>> BulkTransactionStatusEnquiry(string transactionRef, int page = 1, int pageSize = 50, Func<object, string, string, string, Task> cleanUp = null);
        Task<PaymentBaseResponse<string>> GeneratePaymentUrl(GeneratePaymentUrl generatePaymentUrl);
    }
}
