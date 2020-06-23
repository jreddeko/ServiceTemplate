using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using ServiceTemplate.Configuration;
using ServiceTemplate.Services;
using System;
using System.Net.Http;

namespace ServiceTemplate
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IApiService, ApiService>();
        }

        public static IServiceCollection AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<Worker>();
            return services;
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services, IRootConfiguration rootConfiguration)
        {
            services.AddHttpClient(ConfigurationConsts.MyHttpClient, c =>
            {
                Log.Debug("RootConfiguration: {@rootConfiguration}", rootConfiguration);
                var client = new HttpClient();
                var openIdConfiguration = rootConfiguration.OpenIdConfiguration;
                var disco = client.GetDiscoveryDocumentAsync(openIdConfiguration.IdentityServerBaseUrl).Result;

                if (disco.IsError)
                    throw new Exception(disco.Error);

                var tokenResponse = client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = openIdConfiguration.ClientId,
                    ClientSecret = openIdConfiguration.ClientSecret,

                    UserName = openIdConfiguration.Username,
                    Password = openIdConfiguration.Password,
                    Scope = string.Join(" ", openIdConfiguration.Scopes)
                }).Result;

                c.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenResponse.AccessToken);

                // Other settings
                c.BaseAddress = new Uri(rootConfiguration.ResourceConfiguration.ApiBaseUrl);
            });

            return services;
        }

        /// <summary>
        /// Configuration root configuration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static void ConfigureRootConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            services.Configure<OpenIdConfiguration>(configuration.GetSection(ConfigurationConsts.OpenIdConfigurationKey));
            services.Configure<ResourceConfiguration>(configuration.GetSection(ConfigurationConsts.ResourceConfigurationKey));
            services.Configure<TimerConfiguration>(configuration.GetSection(ConfigurationConsts.TimerConfigurationKey));

            services.AddSingleton<IRootConfiguration, RootConfiguration>();
        }
    }
}
