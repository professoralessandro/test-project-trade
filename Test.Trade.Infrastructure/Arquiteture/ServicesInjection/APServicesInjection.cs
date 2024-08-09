#region REFERENCES
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Test.Trade.Infra.Arquiteture.RepositoryInjection;
using Test.Trade.Domain.Interfaces.Data;
using Test.Trade.Domain.Interfaces.Common;
using Test.Trade.Domain.Connector;
using Test.Trade.Domain.Interfaces.Services.Trade;
using Test.Trade.Domain.Servies.Trade;
#endregion

namespace Test.Trade.Infra.Arquiteture.ServicesInjection
{
    public static class APServicesInjection
    {
        #region ADD SERVICES
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            return services
                .RemoveAll<IHttpMessageHandlerBuilderFilter>()
                .RegisterServices()
                .RegisterRepositories()
                .AddServiceNoDependency();
        }
        #endregion

        #region REGFISTER SERVICES
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            // DAPPER
            services.AddScoped<APConnector>();
            services.AddTransient<IAPDWork, APWork>();

            // SERVICES
            services.AddScoped<ITradeRiskService, TradeRiskService>();

            return services;
        }
        #endregion

        #region ADD SERVICE NO DEPENDENCY
        private static IServiceCollection AddServiceNoDependency(this IServiceCollection services)
        {
            var implementationsType = typeof(APServicesInjection).Assembly.GetTypes()
                .Where(t => typeof(IService).IsAssignableFrom(t) && t.BaseType != null);

            foreach (var item in implementationsType)
            {
                services.AddScoped(item);
            }

            return services;

        }
        #endregion
    }
}
