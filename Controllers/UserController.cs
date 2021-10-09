using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SendingDataByEmail.Data.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SendingDataByEmail.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        [HttpGet]
        [Route("GetUserData")]
        [Authorize(Policy = Policies.User)]
        public IActionResult GetUserData()
        {
            return Ok($"This is an normal user ");
        }


        [HttpGet]
        [Route("GetAdminData")]
        [Authorize(Policy = Policies.Admin)]                
        public IActionResult GetAdminData()
        {
            return GetUsers();
        }

        [HttpGet]
        [Route("GetEmail")]
        [Authorize]
        public IActionResult GetEmail()
        {
            Policies.Email = User?.Claims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier, 
                StringComparison.OrdinalIgnoreCase))?.Value;
          return  NoContent();
        }


        public IActionResult GetUsers()
        {
            List<User> users = new List<User>();
            string sqlExpression = "SELECT * FROM User";
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
                            string email = reader.GetString(0);
                            string firstName = reader.GetString(1);
                            string password = reader.GetString(2);
                            string userType = reader.GetString(3);

                            User user = new User(email, firstName, password, userType);
                            users.Add(user);

                        }
                    }
                }
            }
            return new JsonResult(users);
        }
    }
}
