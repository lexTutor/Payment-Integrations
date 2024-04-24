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

        /// <summary>
        /// Retrieves a list of banks asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning a PaymentBaseResponse encapsulating an IList of Bank objects.</returns>
        Task<PaymentBaseResponse<IList<Bank>>> GetBanks();

        /// <summary>
        /// Retrieves account information asynchronously based on the provided AccountEnquiryRequest.
        /// </summary>
        /// <param name="accountEnquiryRequest">The AccountEnquiryRequest object.</param>
        /// <param name="cleanUp">An optional cleanup function. Parameters are in the following order: RequestObject, RawResponseObject, MethodName, RelativeUrl.</param>
        /// <returns>A ValueTask representing the asynchronous operation, returning a PaymentBaseResponse encapsulating an AccountEnquiryResponse object.</returns>

        ValueTask<PaymentBaseResponse<AccountEnquiryResponse>> RetrieveAccountInformation(AccountEnquiryRequest accountEnquiryRequest, Func<object, string, string, string, Task> cleanUp = null);

        /// <summary>
        /// Initiates a single transaction asynchronously based on the provided SingleTransactionInitiationRequest.
        /// </summary>
        /// <param name="singleTransactionInitiationRequest">The SingleTransactionInitiationRequest object.</param>
        /// <param name="cleanUp">An optional cleanup function. Parameters are in the following order: RequestObject, RawResponseObject, MethodName, RelativeUrl.</param>
        /// <returns>A ValueTask representing the asynchronous operation, returning a PaymentBaseResponse encapsulating a SingleTransactionInitiationResponse object.</returns>
        ValueTask<PaymentBaseResponse<SingleTransactionInitiationResponse>> InitiateTransaction(SingleTransactionInitiationRequest singleTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null);

        /// <summary>
        /// Initiates a bulk transaction asynchronously based on the provided BulkTransactionInitiationRequest.
        /// </summary>
        /// <param name="bulkTransactionInitiationRequest">The BulkTransactionInitiationRequest object.</param>
        /// <param name="cleanUp">An optional cleanup function. Parameters are in the following order: RequestObject, RawResponseObject, MethodName, RelativeUrl.</param>
        /// <returns>A ValueTask representing the asynchronous operation, returning a PaymentBaseResponse encapsulating a BulkTransactionInitiationResponse object.</returns>
        ValueTask<PaymentBaseResponse<BulkTransactionInitiationResponse>> InitiateTransaction(BulkTransactionInitiationRequest bulkTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null);

        /// <summary>
        /// Retrieves the status of a single transaction asynchronously based on the provided transaction reference.
        /// </summary>
        /// <param name="transactionRef">The transaction reference.</param>
        /// <param name="cleanUp">An optional cleanup function. Parameters are in the following order: RequestObject, RawResponseObject, MethodName, RelativeUrl.</param>
        /// <returns>A ValueTask representing the asynchronous operation, returning a PaymentBaseResponse encapsulating a TransactionDataResponse object.</returns>
        ValueTask<PaymentBaseResponse<TransactionDataResponse>> SingleTransactionStatusEnquiry(string transactionRef, Func<object, string, string, string, Task> cleanUp = null);

        /// <summary>
        /// Retrieves the status of bulk transactions asynchronously.
        /// </summary>
        /// <param name="transactionRef">The transaction reference (optional, maybe required by the provider).</param>
        /// <param name="page">The page number (default is 1).</param>
        /// <param name="pageSize">The page size (default is 50).</param>
        /// <param name="cleanUp">An optional cleanup function. Parameters are in the following order: RequestObject, RawResponseObject, MethodName, RelativeUrl.</param>
        /// <returns>A ValueTask representing the asynchronous operation, returning a PaymentBaseResponse encapsulating a BulkTransactionStatusResponse object.</returns>
        ValueTask<PaymentBaseResponse<BulkTransactionStatusResponse>> BulkTransactionStatusEnquiry(string transactionRef = "", int page = 1, int pageSize = 50, Func<object, string, string, string, Task> cleanUp = null);

        /// <summary>
        /// Generates a payment URL asynchronously based on the provided GeneratePaymentUrl object.
        /// </summary>
        /// <param name="generatePaymentUrl">The GeneratePaymentUrl object.</param>
        /// <param name="cleanUp">An optional cleanup function. Parameters are in the following order: RequestObject, RawResponseObject, MethodName, RelativeUrl.</param>
        /// <returns>A ValueTask representing the asynchronous operation, returning a PaymentBaseResponse encapsulating a string representing the payment URL.</returns>
        ValueTask<PaymentBaseResponse<string>> GeneratePaymentUrl(GeneratePaymentUrl generatePaymentUrl, Func<object, string, string, string, Task> cleanUp = null);

        /// <summary>
        /// Initiates a bulk settlement transaction asynchronously based on the provided BulkSettleTransactionRequest.
        /// </summary>
        /// <param name="settleTransactionRequest">The BulkTransactionInitiationRequest object.</param>
        /// <param name="cleanUp">An optional cleanup function. Parameters are in the following order: RequestObject, RawResponseObject, MethodName, RelativeUrl.</param>
        /// <returns>A Task representing the asynchronous operation, returning a SettleTransactionResponse object.</returns>

        Task<SettleTransactionResponse> SettleBulkTransaction(BulkSettleTransactionRequest settleTransactionRequest, Func<object, string, string, string, Task> cleanUp = null);

        /// <summary>
        /// Retrieves the status of a settlement transaction asynchronously based on the provided transaction reference (rrr for Remita).
        /// </summary>
        /// <param name="transactionReference">The transaction reference.</param>
        /// <param name="cleanUp">An optional cleanup function. Parameters are in the following order: RequestObject, RawResponseObject, MethodName, RelativeUrl.</param>
        /// <returns>A Task representing the asynchronous operation, returning a SettleBulkTransactionStatusResponse object.</returns>
        Task<SettleBulkTransactionStatusResponse> SettleBulkTransactionStatus(string transactionReference, Func<object, string, string, string, Task> cleanUp = null);
    }
}
