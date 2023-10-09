using demouserdashboard.Data;
using demouserdashboard.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace demouserdashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserDbContext _context;
        public AuthenticationController(UserDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult GetValidateUserLogin([FromBody] Login userlogin)
        {
        
        using (var command = _context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "GetValidateUserLogin";
            command.CommandType = CommandType.StoredProcedure;


            //command.Parameters.AddWithValue("@Email", userlogin.user_email_id);
            command.Parameters.Add(new SqlParameter("@Email", userlogin.email));
            command.Parameters.Add(new SqlParameter("@Password", userlogin.password));
            command.Parameters.Add(new SqlParameter("@UserRole", userlogin.role));

            _context.Database.OpenConnection();
            //HttpContext.Session.SetString("Email", userlogin.user_email_id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    if(userlogin.role=="Watchman")
                    {
                            if (reader[0].ToString()=="False")
                            {
                                var response1 = new { success = false, message = "Login Unsuccessful!" };
                                return Ok(response1);
                            }
                            var response = new { success = true, message = reader[0].ToString() };
                            return Ok(response);
                    }
                    bool Status = (bool)reader["IsValid"];
                    if (Status)
                    {
                        var response = new { success = true, message = "Login Successful!" };
                            return Ok(response);
                    }
                    else
                    {
                        var response = new { success = false, message = "Login Unsuccessful!" };
                            return Ok(response);
                    }
                }
                    return BadRequest();
            }
        }
    }

}
}
