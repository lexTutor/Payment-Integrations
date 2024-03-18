using Integrations.Implementations;
using Integrations.Interfaces;
using Integrations.Model.Common;
using Integrations.Utilities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace Integrations.Extensions
{
    public static class ServiceCollection
    {
        public static void AddRemitaDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region API Servies

            services.AddScoped<IRemitaHttpClient, RemitaHttpClient>();
            services.AddScoped<IRemitaApiService, RemitaApiService>();

            #endregion

            #region Distributed Cache

            var serviceProvider = services.BuildServiceProvider();
            var distributedCache = serviceProvider.GetService<IDistributedCache>();
            if (distributedCache == null)
            {
                services.AddDistributedMemoryCache();
            }

            #endregion

            #region Configuration

            services.AddOptions();
            services.Configure<RemitaConfiguration>(o => configuration.GetSection(RemitaConfiguration.ConfigKey).Bind(o));

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
