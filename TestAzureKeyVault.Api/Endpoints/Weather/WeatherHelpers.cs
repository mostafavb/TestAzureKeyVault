using TestAzureKeyVault.Shared.Models;

internal static class WeatherHelpers
{
    public static async Task<List<WeatherForecast>> GetAllWeather()
    {
        List<WeatherForecast> weatherForecasts = await Task.FromResult(new List<WeatherForecast> {
            new WeatherForecast
            {
                Date=DateOnly.FromDateTime(DateTime.Now),
                Summary="This is a summary",
                TemperatureC = 15
            },
            new WeatherForecast
            {
                Date=DateOnly.FromDateTime(DateTime.Now.AddDays(8)),
                Summary="This is second summary",
                TemperatureC = 22
            }
        });

        return weatherForecasts;
    }
}