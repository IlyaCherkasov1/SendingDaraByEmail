using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendingDataByEmail.Data.Models
{
    public class User
    {
        public User(string email, string firstName, string password, string userType)
        {
            Email = email;
            FirstName = firstName;
            Password = password;
            UserType = userType;
        }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }

    }
}
