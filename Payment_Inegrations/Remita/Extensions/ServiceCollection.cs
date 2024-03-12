using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Remita.Implementations;
using Remita.Interfaces;
using Remita.Model.Common;
using System;
using Remita.Utilities;

namespace Remita.Extensions
{
    public static class ServiceCollection
    {
        public static void AddRemitaDependencies(IServiceCollection services, IConfiguration configuration)
        {
            #region API Servies

            services.AddScoped<IRemitaHttpClient, RemitaHttpClient>();
            services.AddScoped<IRemitaApiService, RemitaApiService>();

            #endregion

            #region Distributed Cache

            var serviceProvider = services.BuildServiceProvider();
            var distributedCache = serviceProvider.GetService<IDistributedCache>();
            if (distributedCache != null)
            {
                services.AddDistributedMemoryCache();
            }

            #endregion

            #region Configuration

            services.AddOptions();
            services.Configure<RemitaConfiguration>(o => configuration.GetSection(RemitaConfiguration.ConfigKey));

            #endregion

            #region HttpClient

            services.AddHttpClient(GenericConstants.RemitaHttpClientName, httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["Remita:Url"]);
                httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            });

            #endregion
        }
    }
}
