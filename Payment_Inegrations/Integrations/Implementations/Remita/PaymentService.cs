using Integrations.Model.ApiModels.Request;
using Integrations.Model.Common;
using System;
using System.Threading.Tasks;

namespace Integrations.Implementations.Remita
{
    public partial class RemitaApiService
    {
        public Task<PaymentBaseResponse<string>> GeneratePaymentUrl(GeneratePaymentUrl generatePaymentUrl)
            => throw new NotImplementedException("There are no APIs available for Remita");
    }
}
