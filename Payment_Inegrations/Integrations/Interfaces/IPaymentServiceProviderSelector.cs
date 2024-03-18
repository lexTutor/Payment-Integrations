using Integrations.Model.Common;

namespace Integrations.Interfaces
{
    public interface IPaymentServiceProviderSelector
    {
        IPaymentProvider GetPaymentService(PaymentProvider paymentProvider);
    }
}
