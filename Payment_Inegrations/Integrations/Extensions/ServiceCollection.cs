using Integrations.Implementations;
using Integrations.Implementations.Paystack;
using Integrations.Implementations.Remita;
using Integrations.Interfaces;
using Integrations.Interfaces.Remita;
using Integrations.Model.Common;
using Integrations.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace Integrations.Extensions
{
    public static class ServiceCollection
    {
        public static void AddPaymentDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region Generic

            services.AddScoped<IPaymentServiceProviderSelector, PaymentServiceProviderSelector>();

            #endregion

            #region API Servies

            services.AddScoped<IRemitaHttpClient, RemitaHttpClient>();
            services.AddScoped<IPaymentProvider, RemitaApiService>();
            services.AddScoped<IPaymentProvider, PaystackApiService>();

            #endregion

            #region Distributed Cache

            try
            {
                services.AddDistributedMemoryCache();
            }
            catch (Exception)
            {
            }

            #endregion

            #region Configuration

            services.AddOptions();
            services.Configure<RemitaConfiguration>(o => configuration.GetSection(RemitaConfiguration.ConfigKey).Bind(o));
            services.Configure<PayStackConfiguration>(o => configuration.GetSection(PayStackConfiguration.ConfigKey).Bind(o));

            #endregion

            #region HttpClient

            services.AddHttpClient(GenericConstants.RemitaHttpClientName, httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["Remita:Url"]);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            #endregion
        }
    }
}
