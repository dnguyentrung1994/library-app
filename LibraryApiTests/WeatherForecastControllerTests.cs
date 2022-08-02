namespace LibraryApiTests;

public class WeatherForecastControllerTests: IClassFixture<ApiWebApplicationFactory>
{
    readonly HttpClient _client;

    public WeatherForecastControllerTests(ApiWebApplicationFactory applicationFactory)
    {
        _client = applicationFactory.CreateClient();
    }

    [Fact]
    public async void GET_retrieves_weather_forecast()
    {
        var response = await _client.GetAsync("./WeatherForecast");
        Assert.Equal(HttpStatusCode.OK,response.StatusCode);
    }
}