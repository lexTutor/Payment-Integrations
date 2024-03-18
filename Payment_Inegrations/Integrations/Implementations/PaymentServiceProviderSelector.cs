using Integrations.Interfaces;
using Integrations.Model.Common;
using System.Collections.Generic;
using System.Linq;

namespace Integrations.Implementations
{
    public class PaymentServiceProviderSelector : IPaymentServiceProviderSelector
    {
        private readonly IEnumerable<IPaymentProvider> _paymentResolutionServices;

        public PaymentServiceProviderSelector(IEnumerable<IPaymentProvider> paymentResolutionServices)
        {
            _paymentResolutionServices = paymentResolutionServices;
        }

        public IPaymentProvider GetPaymentService(PaymentProvider paymentProvider)
            => _paymentResolutionServices.FirstOrDefault(p => p.PaymentProvider == paymentProvider);
    }
}
