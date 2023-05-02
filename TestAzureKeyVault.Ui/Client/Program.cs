using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TestAzureKeyVault.Shared.Services;
using TestAzureKeyVault.Ui;
using TestAzureKeyVault.Ui.Configurations;
using TestAzureKeyVault.Ui.Infrastructures;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var config = builder.Configuration;
var clientName = "TestAzureKeyVault.Ui.ServerAPI";
string apiUrl = "https://localhost:7267";

builder.Services.Configure<ApiSettingOptions>(ops =>
{
    ops.ApiUrl = apiUrl;
    ops.ClientName = clientName;
});

builder.Services.AddScoped<ApiAuthorizationMessageHandler>();

builder.Services.AddHttpClient(clientName, client =>
{
    client.BaseAddress = new Uri(apiUrl);
})
.AddHttpMessageHandler<ApiAuthorizationMessageHandler>();

builder.Services.AddSingleton<ICrypto, Crypto>();

await builder.ImportConfiguration(apiUrl);

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(clientName));

builder.Services.AddMudServices();

builder.Services.AddMsalAuthentication(options =>
{
    config.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add(config["AzureAd:Scope"]!);
});

var b = builder.Build();

await b.RunAsync();