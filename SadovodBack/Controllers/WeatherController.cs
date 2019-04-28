using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SadovodBack.Controllers
{
    public class OpenWeatherMap
    {
        public List<Data> list;
    }

    public class Data
    {
        public Main main { get; set; }
        public List<Weather> weather { get; set; }
        public Clouds clouds { get; set; }
        public Wind wind { get; set; }
        public string dt_txt { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double pressure { get; set; }
        public double sea_level { get; set; }
        public double grnd_level { get; set; }
        public int humidity { get; set; }
        public double temp_kf { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public double deg { get; set; }
    }

    public class Sys
    {
        public string pod { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        [Route("GetRain/")]
        // GET: api/Weather
        [HttpGet]
        public string Get(int lat, int lon)
        {
            var rainTypeDictionary = new Dictionary<string, string>()
            {
                {"light rain","легкий дождь"},
                {"moderate rain","умеренный дождь"},
                {"heavy intensity rain","сильный дождь" },
                {"very heavy rain","очень сильный дождь" },
                {"extreme rain","ливень" },
                {"light intensity shower rain","легкий проливной дождь" },
                {"shower rain","проливной дождь" },
                {"heavy intensity shower rain ","интенсивный проливной дождь" },
                {"ragged shower rain","рваный дождь"}
            };
            var url = string.Format("http://api.openweathermap.org/data/2.5/forecast/hourly?lat={0}&lon={1}&appid=20a742a2d44a20b0e9a9fecfa38609b9",lat,lon);
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            string answer = string.Empty;
            using (Stream s = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(s))
                {
                    answer = reader.ReadLine().ToString();
                }
            }
            var weatherInfo = JsonConvert.DeserializeObject<OpenWeatherMap>(answer);
            var l = weatherInfo.list
                .Where(v => v.weather.FirstOrDefault().main == "Rain" && v.dt_txt.Substring(0, 10) == DateTime.Now.ToString("yyyy-MM-dd") && DateTime.Parse(v.dt_txt) > DateTime.Now)
                .Select(v => new { Weather = v.weather.FirstOrDefault().description, Time = DateTime.Parse(v.dt_txt).Hour }).ToList();
            var prevTime = 0;
            var j = 0;
            var prevWeather = default(string);
            var result = new List<Tuple<string, int, int>>();
            if (l.Count() == 0) return null;
            for (var i = 0; i < l.Count(); i++)
            {
                if (i == 0)
                {
                    prevTime = l[i].Time;
                    prevWeather = l[i].Weather;
                }
                else
                {
                    if (l[i].Weather != prevWeather || l[i].Time != prevTime + 1)
                    {
                        result.Add(Tuple.Create(prevWeather, j + 1, prevTime + 1));
                        prevTime = l[i].Time;
                        prevWeather = l[i].Weather;
                        j = 0;
                    }
                    else
                    {
                        prevTime = l[i].Time;
                        prevWeather = l[i].Weather;
                        j++;
                    }

                }
                if (i == l.Count() - 1)
                {
                    result.Add(Tuple.Create(l[l.Count() - 1].Weather, j + 1, prevTime + 1));
                }
            }
            var superResult = new StringBuilder("Сегодня будет дождь:\n");
            foreach (var e in result)
            {
                superResult.Append(string.Format("{0} с {1}:00 до {2}:00\n", rainTypeDictionary[e.Item1], e.Item3 - e.Item2, e.Item3));
            }
            var asdsad = superResult.ToString();
            return asdsad;
        }
    }
}
