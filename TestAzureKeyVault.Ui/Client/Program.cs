using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using TestAzureKeyVault.Shared.Services;
using TestAzureKeyVault.Ui;
using TestAzureKeyVault.Ui.Infrastructures;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var config = builder.Configuration;
var apiName = "TestAzureKeyVault.API";

builder.Services.AddHttpClient(apiName, client => client.BaseAddress = new Uri("https://localhost:7267"))
   .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();


builder.Services.AddSingleton<ICrypto, Crypto>();

builder.Services.AddScoped(sp =>
{
    var s = sp.GetRequiredService<IHttpClientFactory>().CreateClient(apiName);
    return s;
});

builder.Services.AddMudServices();

await builder.ImportConfiguration("https://localhost:7267");

builder.Services.AddMsalAuthentication(options =>
{
    config.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add(config["AzureAd:Scope"]!);
});

var b = builder.Build();

await b.RunAsync();