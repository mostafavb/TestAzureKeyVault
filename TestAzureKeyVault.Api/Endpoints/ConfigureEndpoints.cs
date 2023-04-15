using TestAzureKeyVault.Api.Endpoints.Post;
using TestAzureKeyVault.Api.Endpoints.Settings;
using TestAzureKeyVault.Api.Endpoints.Weather;

namespace TestAzureKeyVault.Api.Endpoints;

public static class ConfigureEndpoints
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        var weather = app.MapGroup("/api/weather")
            .MapWeather();

        var post = app.MapGroup("/api/post")
            .MapPosts();

        app.MapGroup("/config/appsettings2")
            .MapSettings();

        app.MapGroup("/config/appsettingsencrypted2")
            .MapEncryptSettings();

        return app;
    }
}
