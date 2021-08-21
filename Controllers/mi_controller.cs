using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;

namespace MyWebAPICoreMio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class mi_controllerController : ControllerBase
    {

        public IConfiguration configuration { get; }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<mi_controllerController> _logger;

        public mi_controllerController(ILogger<mi_controllerController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<mi_controller> Get()
        {
            Console.WriteLine("HttpGet");
            var rng = new Random();
            //var sDar = configuration.GetValue<string>("ConnectionStrings:Default");
              var builder = new ConfigurationBuilder()
               .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();
            var connectionString = config["ConnectionStrings:midata"];

            return Enumerable.Range(1, 5).Select(index => new mi_controller
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
            Console.WriteLine("HttpGet id");
            string sSalida = "";
            var builder = new ConfigurationBuilder()
               .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();
            var connectionString = Environment.GetEnvironmentVariable("TEST_NETCORE");//config["ConnectionStrings:Default"];
            Console.WriteLine(connectionString);
            using var connection = new MySqlConnection(connectionString);
            Console.WriteLine("connection");
            connection.Open();
            if (connection!=null) {
                Console.WriteLine("la conection no es null");
                using var command = new MySqlCommand("SELECT * FROM employees;", connection);
                Console.WriteLine("tiro el select");
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()){
                        Console.WriteLine("esta en el while");
                        sSalida = sSalida + reader["last_name"].ToString();
                        Console.WriteLine("ejecuto reader");
                        Console.WriteLine(sSalida);
                        Console.WriteLine("imprimio primer lectura");
                    }
                }
             } else {
                return connectionString;
            }

            connectionString = config["ConnectionStrings:DefaultSQLServer"];
            sSalida = sSalida + connectionString;
            using (SqlConnection con = new SqlConnection(connectionString))  
            {  
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Customers;", con);  
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();  
                while (rdr.Read())  
                {  
                    sSalida = sSalida + rdr[1].ToString();
                    sSalida = sSalida + rdr[2].ToString();
                }       
            }
            return sSalida;
        }


    }
}
