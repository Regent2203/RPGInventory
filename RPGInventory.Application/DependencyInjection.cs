using Microsoft.Extensions.DependencyInjection;
using RPGInventory.Application.Players;

namespace RPGInventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<PlayerService>();

        return services;
    }
}