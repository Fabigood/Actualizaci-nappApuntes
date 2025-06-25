using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ActualizaciónappApuntes.Models.WeatherModels;
using Newtonsoft.Json;

namespace ActualizaciónappApuntes.Repositories
{
    internal class WeatherRepository
    {
        public async Task<WeatherData> GetCurrentLocationWeatherData()
        {
            GeoLocationRepository _geoLocationRepository = new GeoLocationRepository();
            Location location = await _geoLocationRepository.GetCurrentLocationAsync();

            return await GetWeatherDataAsync(location.Latitude, location.Longitude);

        }

        public async Task<WeatherData> GetWeatherDataAsync(double latitude, double longitude)
        {
            string latitude_str = latitude.ToString().Replace(",", ".");
            string longitude_str = longitude.ToString().Replace(",", ".");
            string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude_str}&longitude={longitude_str}&current=temperature_2m,relative_humidity_2m,rain";

            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            WeatherData data = JsonConvert.DeserializeObject<WeatherData>(result);
            return data;
        }
    }
}
