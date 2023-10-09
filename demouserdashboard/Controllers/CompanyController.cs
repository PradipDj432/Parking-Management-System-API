using demouserdashboard.Data;
using demouserdashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.CompilerServices;

namespace demouserdashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UserDbContext _context;
        public CompanyController(UserDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        [HttpGet("dashboard/{companyEmail}")]
        public async Task<IActionResult> GetCompanyDashboard(string companyEmail)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetCompanyDashboard";
                    command.CommandType = CommandType.StoredProcedure;


                    command.Parameters.Add(new SqlParameter("@company_emailid", companyEmail));

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


        [HttpGet]
        [Route("profile/{company_email_id}")]
        public async Task<IActionResult> GetCompanyProfile([FromRoute] string company_email_id)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetCompanyProfile";
                    command.CommandType = CommandType.StoredProcedure;


                    command.Parameters.Add(new SqlParameter("@company_emailid", company_email_id));

                    _context.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        { 
                            var cp = new CompanyEdit();
                            cp.company_email_id = reader.GetString(0);
                            cp.company_password = reader.GetString(1);
                            cp.company_name = reader.GetString(2);
                            cp.company_address = reader.GetString(3);
                            cp.total_parking = reader.GetInt32(4);
                            cp.two_wheel_parking = reader.GetInt32(5);
                            cp.four_wheel_parking = reader.GetInt32(6);
                            cp.two_wheel_charge = reader.GetInt32(7);
                            cp.four_wheel_charge = reader.GetInt32(8);
                            cp.two_wheel_penalty = reader.GetInt32(9);
                            cp.four_wheel_penalty = reader.GetInt32(10);
                            cp.company_contact_no = reader.GetString(11);
                            cp.watchman_emailid = reader.GetString(12);
                            cp.watchman_password = reader.GetString(13);
                            return Ok(cp);
                        }
                        return BadRequest();
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



        [HttpPut("editprofile/{company_email_id}")]
        //[Route("Update")]
        public IActionResult UpdateCompany(string company_email_id, [FromBody] CompanyEdit request)
        {
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("UserConnectionString")))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "GetEditCompanyProfile";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@company_email_id", request.company_email_id);
                    if (!string.IsNullOrEmpty(request.company_password))
                    {
                        cmd.Parameters.AddWithValue("@company_password", request.company_password);
                    }
                    if (!string.IsNullOrEmpty(request.company_name))
                    {
                        cmd.Parameters.AddWithValue("@company_name", request.company_name);
                    }
                    if (!string.IsNullOrEmpty(request.company_address))
                    {
                        cmd.Parameters.AddWithValue("@company_address", request.company_address);
                    }
                    if (request.total_parking != 0)
                    {
                        cmd.Parameters.AddWithValue("@total_parking", request.total_parking);
                    }
                    if (request.two_wheel_parking != 0)
                    {
                        cmd.Parameters.AddWithValue("@two_wheel_parking", request.two_wheel_parking);
                    }
                    if (request.four_wheel_parking != 0)
                    {
                        cmd.Parameters.AddWithValue("@four_wheel_parking", request.four_wheel_parking);
                    }
                    if (request.two_wheel_charge != 0)
                    {
                        cmd.Parameters.AddWithValue("@two_wheel_charge", request.two_wheel_charge);
                    }
                    if (request.four_wheel_charge != 0)
                    {
                        cmd.Parameters.AddWithValue("@four_wheel_charge", request.four_wheel_charge);
                    }
                    if (request.two_wheel_penalty != 0)
                    {
                        cmd.Parameters.AddWithValue("@two_wheel_penalty", request.two_wheel_penalty);
                    }
                    if (request.four_wheel_penalty != 0)
                    {
                        cmd.Parameters.AddWithValue("@four_wheel_penalty", request.four_wheel_penalty);
                    }
                    if (!string.IsNullOrEmpty(request.company_contact_no))
                    {
                        cmd.Parameters.AddWithValue("@company_contact_no", request.company_contact_no);
                    }
                    if (!string.IsNullOrEmpty(request.watchman_password))
                    {
                        cmd.Parameters.AddWithValue("@watchman_password", request.watchman_password);
                    }
                    if (!string.IsNullOrEmpty(request.watchman_emailid))
                    {
                        cmd.Parameters.AddWithValue("@watchman_emailid", request.watchman_emailid);
                    }

                    var rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        return NotFound();
                    }
                    return Ok("Save");
                }
            }
        }
    }
}
