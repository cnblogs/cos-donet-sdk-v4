using Microsoft.Extensions.Configuration;
using QCloud.CosApi.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CosClientServiceCollectionExtensions
    {
        public static IServiceCollection AddCosClient(
            this IServiceCollection services, 
            IConfiguration configuration, 
            string sectionName = "cosClient",
            Action<HttpClient> configureClient = null)
        {
            services.Configure<CosClientOptions>(configuration.GetSection(sectionName));
            if (configureClient == null)
            {
                services.AddHttpClient<CosClient>();
            }
            else
            {
                services.AddHttpClient<CosClient>(configureClient);
            }

            return services;
        }
    }
}
