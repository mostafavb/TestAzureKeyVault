using FastEndpoints;
using TestAzureKeyVault.Api.Services;

namespace TestAzureKeyVault.Api.Endpoints.FastEndpoints;

public class AppSettingsEncrpted : EndpointWithoutRequest
{
    public AppSettingsEncrpted(IClientConfigurationManager manager)
    {
        Manager = manager;
    }

    public IClientConfigurationManager Manager { get; }

    public override void Configure()
    {
        Get("config/appsettingsencrypted/{clientType}");
        AllowAnonymous();
        Description(b => b
          .Produces<string>(200, "application/json")
          .ProducesProblemFE(400)
          .ProducesProblemFE<InternalErrorResponse>(500)
          , clearDefaults: true
      );
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var config = await Manager.GetEncryptedClientConfiguration(Route<string>("clientType"));

        if (config is null)
            await SendErrorsAsync(500);

        await SendOkAsync(config!);
    }
}
