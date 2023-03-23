namespace TestAzureKeyVault.Api.Endpoints.Weather;

public static class Weather
{
    public static RouteGroupBuilder MapWeather(this RouteGroupBuilder group)
    {
        group.MapGet("/", () => WeatherHelpers.GetAllWeather());
        return group;
    }
}
