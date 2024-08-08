#region REFERENCES
using AppMktPlaceV2.Start.Domain.Interfaces.Repository.Trade;
using AppMktPlaceV2.Start.Infrastructure.Repositorys.User;
using Microsoft.Extensions.DependencyInjection;
#endregion

namespace AppMktPlaceV2.Start.Infrastructure.Arquiteture.RepositoryInjection
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
