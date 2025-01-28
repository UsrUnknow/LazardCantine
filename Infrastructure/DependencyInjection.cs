using LazardCantine.Services;

namespace LazardCantine.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IProduitRepository, ProduitRepository>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ClientService>();
        services.AddScoped<IPlateauService, PlateauService>();
        services.AddScoped<ProduitService>();
        return services;
    }
}