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
    public class WatchmanController : ControllerBase
    {
        private readonly UserDbContext _context;
        public WatchmanController(UserDbContext context)
        {
            _context = context;
        }
        
        // watchman dashboard
        [HttpGet("dashboard/{companyEmail}")]
        public async Task<IActionResult> GetWatchmanDashboard(string companyEmail)
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
                            if(value == null)
                                parkingData.Add(metric, 0);
                            else
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

        [HttpGet("viwebooking/{companyEmail}")]
        public async Task<IActionResult> GetViweBooking(string companyEmail)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetViweBookingSP";
                    command.CommandType = CommandType.StoredProcedure;


                    command.Parameters.Add(new SqlParameter("@company_emailid", companyEmail));

                    _context.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        List<object> results = new List<object>();

                        while (reader.Read())
                        {
                            string column1 = (string)reader["username"];
                            TimeSpan entryTimeSpan = (TimeSpan)reader["entrytime"];
                            DateTime entryDateTime = DateTime.Today.Add(entryTimeSpan);
                            TimeSpan exitTimeSpan = (TimeSpan)reader["exittime"];
                            DateTime exitDateTime = DateTime.Today.Add(exitTimeSpan);
                            string entryTime = entryDateTime.ToString("h:mm tt");
                            string exitTime = exitDateTime.ToString("h:mm tt");
                            int column5 = (int)reader["parking_spot_no"];
                            string column7 = reader["user_booking_id"].ToString();
                            string column8 = reader["vehicle_type"].ToString();
                            string column9 = (string)reader["user_contact_no"];
                            string column10 = (string)reader["vihecle_plate"];
                            int column11 = (int)reader["parking_status"];
                            results.Add(new
                            {
                                username = column1,
                                vehicleplate = column10,
                                vehicletype = column8,
                                entrytime = entryTime,
                                exittime = exitTime,
                                slotno = column5,
                                contactno = column9,
                                bookingid = column7,
                                parkingstatus = column11
                            });
                        }

                        return Ok(results);
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


        // change parking status like prebook to live and live to end and prebook to cancel
        [HttpGet("parkingstatus/{bookingid}/{flag}")]
        public async Task<IActionResult> GetChangeParkingStatus(int bookingid, int flag)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetChangeParkingStatusSP";
                    command.CommandType = CommandType.StoredProcedure;


                    command.Parameters.Add(new SqlParameter("@user_booking_id", bookingid));
                    command.Parameters.Add(new SqlParameter("@flag", flag));

                    _context.Database.OpenConnection();

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected > 0)
                    {
                        return Ok(1);
                    }
                    else
                    {
                        return BadRequest(0);
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


        // get patking slip or charge calculate
        [HttpGet("parkingcharge/{bookingid}")]
        public async Task<IActionResult> GetParkingSlip(int bookingid)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetParkingSlipSP";
                    command.CommandType = CommandType.StoredProcedure;


                    command.Parameters.Add(new SqlParameter("@user_booking_id", bookingid));

                    _context.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        int results = 0;

                        while (reader.Read())
                        {


                            results = ((int)reader["parking_charge"]);
                        }

                        return Ok(results);
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
