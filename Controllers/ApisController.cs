using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SendingDataByEmail.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace SendingDataByEmail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApisController : ControllerBase
    {
        private readonly IApisService _apisService;
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ApisController(IApisService apisService)
        {
            _apisService = apisService;
        }

        [HttpGet]
        public ActionResult GetAPIs()
        {

            List<API> apis = new List<API>();
            string sqlExpression = "SELECT * FROM Api";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string url = reader.GetString(2);
                            string city = reader.GetString(3);

                            API api = new API(id, name, url, city);
                            apis.Add(api);
                        }
                    }
                }
            }
            return new JsonResult(apis);
        }


        [HttpGet("{id}")]
        public ActionResult GetAPI(int id)
        {
            API api = null;
            string sqlExpression = "SELECT * FROM Api where ApiId=(@id)";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                SqliteParameter idParam = new SqliteParameter("@id", id);
                command.Parameters.Add(idParam);
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString(1);
                            string url = reader.GetString(2);
                            string city = reader.GetString(3);

                            api = new API(id, name, url, city);
                        }
                    }
                }
            }
            return new JsonResult(api);
        }

        [HttpPut("{id}")]
        public IActionResult PutAPI(int id, string city)
        {

            string sqlExpression = "UPDATE Api SET City=(@city) WHERE ApiId=(@id)";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                SqliteParameter cityParam = new SqliteParameter("@city", city);
                SqliteParameter idParam = new SqliteParameter("@id", id);
                command.Parameters.Add(cityParam);
                command.Parameters.Add(idParam);

                int i = command.ExecuteNonQuery();
            }
            return NoContent();
        }


    }
}
