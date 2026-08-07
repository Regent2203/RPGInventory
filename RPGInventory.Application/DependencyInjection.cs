using Microsoft.Extensions.DependencyInjection;
using RPGInventory.Application.Crafting;
using RPGInventory.Application.Players;
using RPGInventory.Application.Shop;

namespace RPGInventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<PlayerService>();
        services.AddScoped<CraftService>();
        services.AddScoped<ShopService>();

        return services;
    }
}