using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiOwinSecure.Models;

namespace WebApiOwinSecure.Data
{
    public class UsersData
    {
        private static List<User> _users = new List<User>();
        
        public static List<User> GetUsers()
        {
            _users.Clear();
            string connectionString = "Server=192.168.1.21;Port=3306;Database=DenemeDB;Uid=remote_user;Password=rootroot;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM users";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string firstName = reader["firstname"].ToString();
                            string lastName = reader["lastname"].ToString();
                            User user = new User(id, firstName, lastName);
                            _users.Add(user);
                        }
                    }
                }
            }

            return _users;
        }
    }
}