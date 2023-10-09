using demouserdashboard.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace demouserdashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UserDbContext _context;
        public AdminController(UserDbContext context)
        {
            _context = context;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetAdminDashboard";
                    command.CommandType = CommandType.StoredProcedure;


                    _context.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        var parkingData = new Dictionary<string, object>();

                        while (reader.Read())
                        {
                            var metric = reader.GetString(0);
                            var value = reader.GetInt32(1);

                            parkingData.Add(metric, value);
                        }

                        return Ok(parkingData);
                    }
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database error");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

    }
}
