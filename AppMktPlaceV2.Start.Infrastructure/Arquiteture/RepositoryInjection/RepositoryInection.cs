#region REFERENCES
using Microsoft.Extensions.DependencyInjection;
using Test.Trade.Domain.Interfaces.Repository.Trade;
using Test.Trade.Infra.Repositorys.Trade;
#endregion

namespace Test.Trade.Infra.Arquiteture.RepositoryInjection
{
    public static class RepositoryInection
    {
        #region REGISTER REPOSITORIES
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            return services.
                AddScoped<ITradeRiskRepository, TradeRiskRepository>();
        }
        #endregion
    }
}
