

using TestAzureKeyVault.Shared.Models;

namespace TestAzureKeyVault.Api.Services;

public interface IClientConfigurationManager
{
    Task<ClientConfiguration> GetClientConfiguration(string? clientType);
    Task<string> GetEncryptedClientConfiguration(string? clientType);
}
