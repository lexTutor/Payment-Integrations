using Integrations.Model.Api.Request;
using Integrations.Model.Api.Response;
using Integrations.Model.Common;
using PayStack.Net;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Integrations.Implementations.Paystack
{
    public partial class PaystackApiService
    {
        public Task<PaymentBaseResponse<BulkTransactionStatusResponse>> BulkTransactionStatusEnquiry(string transactionRef, int page = 1, int pageSize = 50, Func<object, string, string, string, Task> cleanUp = null)
        {
            var response = _payStackApi.Transfers.ListTransfers(pageSize, page);

            return Task.FromResult(PaymentBaseResponse<BulkTransactionStatusResponse>.Successful("successful", new BulkTransactionStatusResponse
            {
                Transactions = response.Data.Select(t => new BulkPaymentTransactionDataResponse
                {
                    Amount = t.Amount,
                    Currency = t.Currency,
                    DestinationAccount = t.Recipient?.Details?.AccountNumber,
                    SourceAccount = t.Source,
                    PaymentStatus = t.Status,
                    DestinationBankCode = t.Recipient?.Details?.BankCode,
                    PaymentDate = t.UpdatedAt,
                    TransactionDate = t.CreatedAt,
                    TransactionDescription = t.Reason,
                    TransactionRef = t.TransferCode
                }).ToList()
            }));
        }

        public Task<PaymentBaseResponse<SingleTransactionInitiationResponse>> InitiateTransaction(SingleTransactionInitiationRequest singleTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null)
        {
            if (singleTransactionInitiationRequest.Amount <= 0)
                return Task.FromResult(PaymentBaseResponse<SingleTransactionInitiationResponse>.Failed("Failed", error: $"Invalid {nameof(singleTransactionInitiationRequest.Amount)}"));

            if (string.IsNullOrWhiteSpace(singleTransactionInitiationRequest.DestinationAccount))
                return Task.FromResult(PaymentBaseResponse<SingleTransactionInitiationResponse>.Failed("Failed", error: $"Invalid {nameof(singleTransactionInitiationRequest.DestinationAccount)}"));

            if (string.IsNullOrWhiteSpace(singleTransactionInitiationRequest.Currency))
                return Task.FromResult(PaymentBaseResponse<SingleTransactionInitiationResponse>.Failed("Failed", error: $"Invalid {nameof(singleTransactionInitiationRequest.Currency)}"));

            var response = _payStackApi.Transfers.InitiateTransfer(amount: (int)Math.Ceiling(singleTransactionInitiationRequest.Amount),
                recipientCode: singleTransactionInitiationRequest.DestinationAccount, currency: singleTransactionInitiationRequest.Currency);

            if (response.Status)
            {
                return Task.FromResult(PaymentBaseResponse<SingleTransactionInitiationResponse>.Successful(response.Message));
            }
            else
            {
                return Task.FromResult(PaymentBaseResponse<SingleTransactionInitiationResponse>.Failed(response.Message));
            }
        }

        public Task<PaymentBaseResponse<BulkTransactionInitiationResponse>> InitiateTransaction(BulkTransactionInitiationRequest bulkTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null)
        {
            if (bulkTransactionInitiationRequest.Transactions.Any(t => t.Amount <= 0))
                return Task.FromResult(PaymentBaseResponse<BulkTransactionInitiationResponse>.Failed("Failed", error: "One or more Amount specification is invalid"));

            if (bulkTransactionInitiationRequest.Transactions.Any(t => string.IsNullOrWhiteSpace(t.DestinationAccount)))
                return Task.FromResult(PaymentBaseResponse<BulkTransactionInitiationResponse>.Failed("Failed", error: $"One or more DestinationAccount specification is invalid"));

            if (string.IsNullOrWhiteSpace(bulkTransactionInitiationRequest.Currency))
                return Task.FromResult(PaymentBaseResponse<BulkTransactionInitiationResponse>.Failed("Failed", error: $"Invalid {nameof(bulkTransactionInitiationRequest.Currency)}"));

            var requestData = bulkTransactionInitiationRequest.Transactions.Select(t => new BulkTransferEntry
            {
                Amount = (int)t.Amount,
                Recipient = t.DestinationAccount
            });

            var response = _payStackApi.Transfers.InitiateBulkTransfer(requestData, currency: bulkTransactionInitiationRequest.Currency);

            if (response.Status)
            {
                return Task.FromResult(PaymentBaseResponse<BulkTransactionInitiationResponse>.Successful(response.Message));
            }
            else
            {
                return Task.FromResult(PaymentBaseResponse<BulkTransactionInitiationResponse>.Failed(response.Message));
            }
        }

        public Task<PaymentBaseResponse<TransactionDataResponse>> SingleTransactionStatusEnquiry(string transactionRef, Func<object, string, string, string, Task> cleanUp = null)
        {
            var response = _payStackApi.Transfers.FetchTransfer(transactionRef);

            if (response.Status && response?.Data != null)
            {
                return Task.FromResult(PaymentBaseResponse<TransactionDataResponse>.Successful(response.Message, new TransactionDataResponse
                {
                    Amount = response.Data.Amount,
                    Currency = response.Data.Currency,
                    DestinationAccount = response.Data.Recipient?.Details?.AccountNumber,
                    SourceAccount = response.Data.Source,
                    PaymentStatus = response.Data.Status,
                    DestinationBankCode = response.Data.Recipient?.Details?.BankCode,
                    PaymentDate = response.Data.UpdatedAt,
                    TransactionDate = response.Data.CreatedAt,
                    TransactionDescription = response.Data.Reason,
                    TransactionRef = response.Data.TransferCode
                }));
            }
            else
            {
                return Task.FromResult(PaymentBaseResponse<TransactionDataResponse>.Failed(response.Message));
            }
        }
    }
}
