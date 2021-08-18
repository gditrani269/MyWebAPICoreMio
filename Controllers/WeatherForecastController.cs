using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace MyWebAPICoreMio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        public IConfiguration configuration { get; }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            //var sDar = configuration.GetValue<string>("ConnectionStrings:Default");
              var builder = new ConfigurationBuilder()
               .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();
            var connectionString = config["ConnectionStrings:midata"];

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                midadito = connectionString //"das" //configuration.GetValue<string>("ConnectionStrings:otro")//"das"
                //midadito = configuration.GetSection("ConnectionStrings").GetSection("Default").Value
            })
            .ToArray();
        }

        [HttpGet ("{id}")]
        public ActionResult<String> Get(int id)
        {
            Console.WriteLine("Hello from Mac");
            return "test" + id.ToString () + " aca toy: " + Environment.GetEnvironmentVariable("TEST_NETCORE");
        }

        [HttpGet ("/{id2}")]
        public ActionResult<String> GetItem(int id2)
        {
            return "test sar " + id2.ToString () ;
        }

    }
}
