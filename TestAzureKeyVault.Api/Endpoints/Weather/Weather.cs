namespace TestAzureKeyVault.Api.Endpoints.Weather;

public static class Weather
{
    public static RouteGroupBuilder MapWeather(this RouteGroupBuilder group)
    {
        group.MapGet("/", () => WeatherHelpers.GetAllWeather())
            .RequireAuthorization(Authorization.Policies.AdminsGroup);
        return group;
    }
}
