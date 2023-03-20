using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiOwinSecure.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public User(int id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public User()
        {

        }
    }
}