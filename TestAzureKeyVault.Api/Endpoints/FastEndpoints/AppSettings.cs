using FastEndpoints;
using TestAzureKeyVault.Api.Services;
using TestAzureKeyVault.Shared.Models;

namespace TestAzureKeyVault.Api.Endpoints.FastEndpoints;

public sealed class AppSettings : EndpointWithoutRequest
{
    public AppSettings(IClientConfigurationManager manager)
    {
        Manager = manager;
    }

    public IClientConfigurationManager Manager { get; }

    public override void Configure()
    {
        Get("config/appsettings/{clientType}");
        AllowAnonymous();
        Description(b => b
          .Produces<ClientConfiguration>(200, "application/json")
          .ProducesProblemFE(400)
          .ProducesProblemFE<InternalErrorResponse>(500)
          , clearDefaults: true
      );
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var config = await Manager.GetClientConfiguration(Route<string>("clientType"));

        if (config is null)
            await SendErrorsAsync(500);

        await SendOkAsync(config!);
    }
}
