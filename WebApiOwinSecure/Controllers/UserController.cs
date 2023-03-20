using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using WebApiOwinSecure.Data;
using WebApiOwinSecure.Models;

namespace WebApiOwinSecure.Controllers
{
    public class UserController : ApiController
    {
        private List<User> _users = UsersData.GetUsers();
        
        MySqlConnection connection = new MySqlConnection("Server=192.168.1.21;Port=3306;Database=DenemeDB;Uid=remote_user;Password=rootroot;");
        /// <summary>
        /// Get All Users
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("api/users")]
        public IHttpActionResult Get()
        {
            if (_users.Count == 0)
            {
                return Ok("Database Disconnected");
            }
            else
            {
                return Ok(_users);
            }
            
            //return Ok("Now server time is: " + DateTime.Now.ToString());
        }

        [Authorize]
        [HttpGet]
        [Route("api/data/authenticate")]
        public IHttpActionResult GetForAuthenticate()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello " + identity.Name);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/data/authorize")]
        public IHttpActionResult GetForAdmin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);
            return Ok("Hello " + identity.Name + " Role: " + string.Join(",", roles.ToList()));
        }
        [Authorize]
        [HttpPost]
        [Route("api/users")]
        public IHttpActionResult Post([FromBody] User user)
        {
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Users (firstname, lastname) VALUES (@firstname, @lastname);";
            command.Parameters.AddWithValue("@firstname", user.FirstName);
            command.Parameters.AddWithValue("@lastname", user.LastName);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
                return BadRequest("Kullanıcı eklenirken bir hata oluştu.");
            }
        }
        [Authorize]
        [HttpPut]
        [Route("api/users")]
        public IHttpActionResult Put([FromBody] User user)
        {
            var editedUser = _users.FirstOrDefault(x => x.Id == user.Id);
            if (editedUser != null)
            {
                editedUser.FirstName = user.FirstName;
                editedUser.LastName = user.LastName;

                connection.Open();

                string query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName WHERE Id = @Id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@FirstName", user.FirstName);
                command.Parameters.AddWithValue("@LastName", user.LastName);
                int rowsAffected = command.ExecuteNonQuery();

                connection.Close();


                return Ok(user);
            }
            else
            {
                return Ok("İzin yok");
            }
        }
    }

    
}