using TestAzureKeyVault.Api.Endpoints.Post;
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

        return app;
    }
}
