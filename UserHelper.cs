using Microsoft.Data.Sqlite;
using SendingDataByEmail.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendingDataByEmail
{
    public class UserHelper
    {
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();
            string sqlExpression = "SELECT * FROM User";
            using (var connection = new SqliteConnection("Data Source=API.db"))
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
            return users;
        }
    }
}
