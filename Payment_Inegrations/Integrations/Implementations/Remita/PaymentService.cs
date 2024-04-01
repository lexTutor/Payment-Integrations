using Integrations.Model.Api.Request;
using Integrations.Model.Common;
using System;
using System.Threading.Tasks;

namespace Integrations.Implementations.Remita
{
    public partial class RemitaApiService
    {
        public ValueTask<PaymentBaseResponse<string>> GeneratePaymentUrl(GeneratePaymentUrl generatePaymentUrl, Func<object, string, string, string, Task> cleanUp = null)
            => throw new NotImplementedException("GeneratePaymentUrl API is not available for Remita");
    }
}
