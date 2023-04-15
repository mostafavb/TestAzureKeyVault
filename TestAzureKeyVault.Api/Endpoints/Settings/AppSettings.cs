using TestAzureKeyVault.Api.Services;
using TestAzureKeyVault.Shared.Contracts;

namespace TestAzureKeyVault.Api.Endpoints.Settings;

public static class AppSettings
{
    public static RouteGroupBuilder MapSettings(this RouteGroupBuilder group)
    {        
        group.MapGet("/",  async (string? clientType, IClientConfigurationManager manager ) =>
        {
            var config = await manager.GetClientConfiguration(clientType);
            return config;
        });

        return group;
    }

    public static RouteGroupBuilder MapEncryptSettings(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (string? clientType, IClientConfigurationManager manager) =>
        {
            var config = await manager.GetEncryptedClientConfiguration(clientType);
            return config;
        });

        return group;
    }
}
