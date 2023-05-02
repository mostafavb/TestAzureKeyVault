using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using TestAzureKeyVault.Ui.Configurations;

namespace TestAzureKeyVault.Ui.Infrastructures;

public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public ApiAuthorizationMessageHandler(IAccessTokenProvider provider,
        NavigationManager navigationManager,IConfiguration configuration, IOptions<ApiSettingOptions> apiOption)
        : base(provider, navigationManager)
    {
        var url = apiOption.Value.ApiUrl;// configuration["ApiUrl"];
        ConfigureHandler(
            authorizedUrls: new[] { url },
            scopes: new[] { configuration["AzureAd:Scope"] }
            );
    }
}
