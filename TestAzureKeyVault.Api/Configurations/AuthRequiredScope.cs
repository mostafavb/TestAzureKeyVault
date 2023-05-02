using Microsoft.Identity.Web;

namespace TestAzureKeyVault.Api.Configurations;

public class AuthRequiredScopeMetadata : IAuthRequiredScopeMetadata
{
    public AuthRequiredScopeMetadata(params string[] scope)
    {
        AcceptedScope = scope;
    }

    public string[]? AcceptedScope { get; }

    public string? RequiredScopesConfigurationKey { get; }
}
