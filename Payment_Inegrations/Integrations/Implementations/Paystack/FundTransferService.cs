using Integrations.Model.Api.Request;
using Integrations.Model.Api.Response;
using Integrations.Model.Common;
using PayStack.Net;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integrations.Implementations.Paystack
{
    public partial class PaystackApiService
    {
        public async ValueTask<PaymentBaseResponse<BulkTransactionStatusResponse>> BulkTransactionStatusEnquiry(string transactionRef = "", int page = 1, int pageSize = 50, Func<object, string, string, string, Task> cleanUp = null)
        {
            var response = _payStackApi.Transfers.ListTransfers(pageSize, page);

            var transactions = string.IsNullOrWhiteSpace(transactionRef) ? response.Data : response.Data.Where(x => x.TransferCode == transactionRef);

            if (cleanUp != null)
            {
                await cleanUp(transactionRef, response.RawJson, HttpMethod.Get.ToString(), nameof(_payStackApi.Transfers.ListTransfers));
            }

            return PaymentBaseResponse<BulkTransactionStatusResponse>.Successful("successful", new BulkTransactionStatusResponse
            {
                Transactions = transactions.Select(t => new BulkPaymentTransactionDataResponse
                {
                    Amount = t.Amount / 100,
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
            });
        }

        public async ValueTask<PaymentBaseResponse<SingleTransactionInitiationResponse>> InitiateTransaction(SingleTransactionInitiationRequest singleTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null)
        {
            if (singleTransactionInitiationRequest.Amount <= 0)
                return PaymentBaseResponse<SingleTransactionInitiationResponse>.Failed("Failed", error: $"Invalid {nameof(singleTransactionInitiationRequest.Amount)}");

            if (string.IsNullOrWhiteSpace(singleTransactionInitiationRequest.DestinationAccount))
                return PaymentBaseResponse<SingleTransactionInitiationResponse>.Failed("Failed", error: $"Invalid {nameof(singleTransactionInitiationRequest.DestinationAccount)}");

            if (string.IsNullOrWhiteSpace(singleTransactionInitiationRequest.Currency))
                return PaymentBaseResponse<SingleTransactionInitiationResponse>.Failed("Failed", error: $"Invalid {nameof(singleTransactionInitiationRequest.Currency)}");

            var response = _payStackApi.Transfers.InitiateTransfer(amount: (int)Math.Ceiling(singleTransactionInitiationRequest.Amount * 100),
                recipientCode: singleTransactionInitiationRequest.DestinationAccount, currency: singleTransactionInitiationRequest.Currency);

            if (cleanUp != null)
            {
                await cleanUp(singleTransactionInitiationRequest, response.RawJson, HttpMethod.Post.ToString(), nameof(_payStackApi.Transfers.InitiateTransfer));
            }

            if (response.Status)
            {
                return PaymentBaseResponse<SingleTransactionInitiationResponse>.Successful(response.Message);
            }
            else
            {
                return PaymentBaseResponse<SingleTransactionInitiationResponse>.Failed(response.Message);
            }
        }

        public async ValueTask<PaymentBaseResponse<BulkTransactionInitiationResponse>> InitiateTransaction(BulkTransactionInitiationRequest bulkTransactionInitiationRequest, Func<object, string, string, string, Task> cleanUp = null)
        {
            if (bulkTransactionInitiationRequest.Transactions.Any(t => t.Amount <= 0))
                return PaymentBaseResponse<BulkTransactionInitiationResponse>.Failed("Failed", error: "One or more Amount specification is invalid");

            if (bulkTransactionInitiationRequest.Transactions.Any(t => string.IsNullOrWhiteSpace(t.DestinationAccount)))
                return PaymentBaseResponse<BulkTransactionInitiationResponse>.Failed("Failed", error: $"One or more DestinationAccount specification is invalid");

            if (string.IsNullOrWhiteSpace(bulkTransactionInitiationRequest.Currency))
                return PaymentBaseResponse<BulkTransactionInitiationResponse>.Failed("Failed", error: $"Invalid {nameof(bulkTransactionInitiationRequest.Currency)}");

            var requestData = bulkTransactionInitiationRequest.Transactions.Select(t => new BulkTransferEntry
            {
                Amount = (int)t.Amount * 100,
                Recipient = t.DestinationAccount
            });

            var response = _payStackApi.Transfers.InitiateBulkTransfer(requestData, currency: bulkTransactionInitiationRequest.Currency);

            if (cleanUp != null)
            {
                await cleanUp(bulkTransactionInitiationRequest, response.RawJson, HttpMethod.Get.ToString(), nameof(_payStackApi.Transfers.InitiateBulkTransfer));
            }

            if (response.Status)
            {
                return PaymentBaseResponse<BulkTransactionInitiationResponse>.Successful(response.Message);
            }
            else
            {
                return PaymentBaseResponse<BulkTransactionInitiationResponse>.Failed(response.Message);
            }
        }

        public async ValueTask<PaymentBaseResponse<TransactionDataResponse>> SingleTransactionStatusEnquiry(string transactionRef, Func<object, string, string, string, Task> cleanUp = null)
        {
            var response = _payStackApi.Transfers.FetchTransfer(transactionRef);

            if (cleanUp != null)
            {
                await cleanUp(transactionRef, response.RawJson, HttpMethod.Get.ToString(), nameof(_payStackApi.Transfers.ListTransfers));
            }

            if (response.Status && response.Data != null && response.Data.Status == "success")
            {
                return PaymentBaseResponse<TransactionDataResponse>.Successful(response.Message, new TransactionDataResponse
                {
                    Amount = response.Data.Amount / 100,
                    Currency = response.Data.Currency,
                    DestinationAccount = response.Data.Recipient?.Details?.AccountNumber,
                    SourceAccount = response.Data.Source,
                    PaymentStatus = response.Data.Status,
                    DestinationBankCode = response.Data.Recipient?.Details?.BankCode,
                    PaymentDate = response.Data.UpdatedAt,
                    TransactionDate = response.Data.CreatedAt,
                    TransactionDescription = response.Data.Reason,
                    TransactionRef = response.Data.TransferCode
                });
            }
            else
            {
                return PaymentBaseResponse<TransactionDataResponse>.Failed(response.Message);
            }
        }
    }
}
