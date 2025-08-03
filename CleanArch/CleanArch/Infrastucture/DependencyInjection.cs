using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CleanArch.Infrastructure.Data;
using CleanArch.Infrastructure.Repositories;
using CleanArch.Application.Abstractions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString); 
        });

        services.AddScoped<IProductRepository, ProductRepository>();
        return services;
    }
}