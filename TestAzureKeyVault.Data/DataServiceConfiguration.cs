using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using TestAzureKeyVault.Data.Services;
using TestAzureKeyVault.Shared.Contracts;

namespace TestAzureKeyVault.Data;

public static class DataServiceConfiguration
{
    public static IServiceCollection DataServicesRegistration(this IServiceCollection services, IConfiguration configuration)
    {
        var sqlConnection = configuration.GetConnectionString("TestConnectionString");
        services.AddDbContext<TestDbContext>(options =>
        {
            options.UseSqlServer(sqlConnection);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddScoped(typeof(IGeneralRepository<>), typeof(GeneralRepository<>));
        services.AddScoped<IPostRepository, PostRepository>();

        return services;
    }
}