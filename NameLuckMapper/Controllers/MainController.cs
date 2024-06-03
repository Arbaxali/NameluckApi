using Microsoft.AspNetCore.Mvc;
using NameLuckMapper.Models;
using NameLuckMapper.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NameLuckMapper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MainController> _logger;

        public MainController(ILogger<MainController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost(Name = "InsertNameAndDob")]
        public ActionResult<NameDobModel> InsertNameDob([FromBody] NameDobModel payload)
        {
            if (payload == null)
            {
                return BadRequest("Payload is null");
            }

            var result = NameMapper.Mapper(payload);

            //var jsonSettings = new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore,
            //    Formatting = Formatting.Indented // You can adjust formatting as needed
            //};

            //var jsonResult = JsonConvert.SerializeObject(result, jsonSettings);
            //var jObjectResult = JObject.Parse(jsonResult);

            var jsonResponse = result.ToString(); // Convert JObject to JSON string

            return Content(jsonResponse, "application/json");

           

           
        }



    }
}
