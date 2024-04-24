# .NET Payment and Fintech Integration Library

This library aims to centralize online payment and digital funds transsfer integrations in .NET applications, providing developers with a seamless and standardized way to incorporate payment processing features into their projects. The scope of this project is stricly about unifying integrations that are generic and cut across multiple providers it does not intend to cover all possible apis provided by the individual providers.

## Features

- **Unified API**: Offers a unified API interface for various payment gateways and fintech services.
- **Easy Integration**: Simplifies the integration process, reducing development time and effort.
- **Extensible**: Provides a framework for adding new payment gateways and fintech services as needed.
- **Best Practices**: Implements industry-standard coding best practices.
- **Multi-Platform Targeting**: The library is built using .NET standard 2.0 as such it can be used by both .NET framework and .NET core applications.

## Installation

To use this library in your .NET project, follow these steps:

1. **NuGet Package Manager**: Open your project in Visual Studio and go to `Tools > NuGet Package Manager > Manage NuGet Packages for Solution`.

2. **Search for Package**: In the NuGet Package Manager, search for the package named `Generic.Payment.Integrations`.

3. **Install Package**: Select the package from the search results and click `Install` to add it to your project.

Alternatively, you can install the package via the NuGet Package Manager Console using the following command:

```bash
Install-Package Generic.Payment.Integrations
```

## Usage

Once the package is installed, you can start using the library to integrate payment and fintech services into your application. Here's an example of how to get started:

The following platforms have been integrated thus far
- Paystack (https://paystack.com/docs/api/)
- Remita (https://api.remita.net/)

The following apis have been currently integrated

| Provider                       | Remita | Paystack |
|--------------------------------|--------|----------|
| Online Payment                 | No     | Yes      |
| Fund Transfer                  | Yes    | Yes      |
| Bulk Fund Transfer             | Yes    | Yes      |
| Account Enquiry                | Yes    | Yes      |
| Transaction Enquiry            | Yes    | Yes      |
| Bulk Transaction Enquiry       | Yes    | Yes      |
| Settlement Transaction         | Yes    | No       |
| Settlement Transaction Enquiry | Yes    | No       |
| Banks                          | Yes    | Yes      |



```csharp
using Microsoft.AspNetCore.Mvc;
using Integrations.Interfaces;
using Integrations.Model.Common;
using Integrations.Model.Api.Request;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentServiceProviderSelector _serviceProviderSelector;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(IPaymentServiceProviderSelector serviceProviderSelector,
        ILogger<WeatherForecastController> logger)
    {
        _serviceProviderSelector = serviceProviderSelector;
        _logger = logger;
    }

    [HttpGet(Name = "Banks")]
    public async Task<IActionResult> GetBanks()
    {
        var provider = _serviceProviderSelector.GetPaymentService(PaymentProvider.Remita);
        var banks = await provider.GetBanks();

        return Ok(banks);
    }

    [HttpPost(Name = "InitiateBulkTransaction")]
    public async Task<IActionResult> Initiate(BulkTransactionInitiationRequest bulkTransactionInitiationRequest)
    {
        var provider = _serviceProviderSelector.GetPaymentService(PaymentProvider.Remita);
        var transactionResponse = await provider.InitiateTransaction(bulkTransactionInitiationRequest, CleanUp);

        return Ok(transactionResponse);
    }

    
    private Task CleanUp(object requestObject, string rawResponse, string httpMethod, string relativeUrl)
    {
        _logger.LogInformation(relativeUrl, new
        {
            httpMethod,
            rawResponse,
            requestObject
        });

        return Task.CompletedTask;
    }
}
```

## Contributing

Contributions to this library are welcome! If you have ideas for improvements or new features, feel free to fork the repository, make your changes, and submit a pull request.

## License

This library is licensed under the MIT License.

---
