using demouserdashboard.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using Microsoft.AspNetCore.SignalR;
using demouserdashboard.Models;
using System.Globalization;

namespace demouserdashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDashboardController : ControllerBase
    {
        private readonly UserDbContext _context;
        public UserDashboardController(UserDbContext context)
        {
            _context = context;
        }
        
        //prebook card
        [HttpGet]
        [Route("prebook/{useremail}")]
        public async Task<IActionResult> GetPreBookCard(string useremail)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetUserPreBookCard";
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@useremail", useremail));

                    _context.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        List<object> results = new List<object>();

                        while (reader.Read())
                        {
                            string column1 = (string)reader["username"];
                            string column2 = (string)reader["name"];
                            TimeSpan entryTimeSpan = (TimeSpan)reader["entrytime"];
                            DateTime entryDateTime = DateTime.Today.Add(entryTimeSpan);
                            TimeSpan exitTimeSpan = (TimeSpan)reader["exittime"];
                            DateTime exitDateTime = DateTime.Today.Add(exitTimeSpan);
                            string entryTime = entryDateTime.ToString("h:mm tt");
                            string exitTime = exitDateTime.ToString("h:mm tt");
                            DateTime parkingDate = (DateTime)reader["parking_date"];
                            string column3 = parkingDate.ToString("yyyy-MM-dd");
                            int column5 = (int)reader["parking_spot_no"];
                            string column6 = (string)reader["address"];
                            string column7 = reader["user_booking_id"].ToString();


                            results.Add(new
                            {
                                username = column1,
                                companyname = column2,
                                entrytime = entryTime,
                                exittime = exitTime,
                                bookdate = column3,
                                slotno = column5,
                                address = column6,
                                bookingid = column7
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
               
                return BadRequest();
            }
        }


        // Get ALL Company on user dashboard
        [HttpGet]
        [Route("allcompany")]
        public async Task<IActionResult> GetAllCompany()
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetAllCompanySP";
                    command.CommandType = CommandType.StoredProcedure;
                    _context.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        List<object> results = new List<object>();

                        while (reader.Read())
                        {
                            string column1 = (string)reader["name"];
                        
                            string column6 = (string)reader["address"];

                            string column3 = (string)reader["company_emailid"];

                            results.Add(new
                            {
                                companyemail = column3,
                                companyname = column1,
                                companyaddress = column6
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
                
                return BadRequest();
            }
        }


        // Get Search Company on user dashboard
        [HttpGet]
        [Route("searchcompany/{companyname}")]
        public async Task<IActionResult> GetSearchCompany(string companyname)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetSearchCompanySP";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@companyname",companyname));
                    _context.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        List<object> results = new List<object>();

                        while (reader.Read())
                        {
                            string column1 = (string)reader["name"];

                            string column6 = (string)reader["address"];

                            string column3 = (string)reader["company_emailid"];

                            results.Add(new
                            {
                                companyemail = column3,
                                companyname = column1,
                                companyaddress = column6
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

                return BadRequest();
            }
        }

        [HttpPost]
        [Route("searchslot")]
        public async Task<IActionResult> GetSearchSlot(InUseSlot obj1)
        {
            try
            {
                using(var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    TimeSpan etime = TimeSpan.ParseExact(obj1.entrytime, "hh\\:mm", CultureInfo.InvariantCulture);
                    TimeSpan extime = TimeSpan.ParseExact(obj1.exittime, "hh\\:mm", CultureInfo.InvariantCulture);
                    command.CommandText = "GetInUseSlotSP";
                    command.CommandType = CommandType.StoredProcedure;
                    
                    command.Parameters.Add(new SqlParameter("@company_emailid", obj1.company_emailid));
                    command.Parameters.Add(new SqlParameter("@entrytime", etime));
                    command.Parameters.Add(new SqlParameter("@exittime", extime));
                    command.Parameters.Add(new SqlParameter("@vehicletype", obj1.vehicle_type));
                    _context.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        List<int> results = new List<int>();

                        while (reader.Read())
                        {


                            results.Add((int)reader["parking_spot_no"]);
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

                return BadRequest();
            }
        }

        // add booking
        [HttpPost]
        [Route("addbooking")]
        public async Task<IActionResult> PostAddBooking(InUseSlot obj1)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    TimeSpan etime = TimeSpan.ParseExact(obj1.entrytime, "hh\\:mm", CultureInfo.InvariantCulture);
                    TimeSpan extime = TimeSpan.ParseExact(obj1.exittime, "hh\\:mm", CultureInfo.InvariantCulture);
                    command.CommandText = "GetAddBookingSP";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@company_emailid", obj1.company_emailid));
                    command.Parameters.Add(new SqlParameter("@user_emailid", obj1.user_emailid));
                    command.Parameters.Add(new SqlParameter("@vehicle_plate", obj1.vehicle_plate));
                    command.Parameters.Add(new SqlParameter("@parking_spot_no", obj1.parking_spot_no));
                    command.Parameters.Add(new SqlParameter("@entrytime", etime));
                    command.Parameters.Add(new SqlParameter("@exittime", extime));
                    command.Parameters.Add(new SqlParameter("@vehicle_type", obj1.vehicle_type));
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
                return BadRequest();
            }
        }

        // inuse and available parking
        [HttpPost]
        [Route("totalparking")]
        public async Task<IActionResult> GetTotalParking(InUseSlot obj1)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "GetTotalParkingForInAvailableSP";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@company_emailid", obj1.company_emailid));
                    command.Parameters.Add(new SqlParameter("@vehicletype", obj1.vehicle_type));
                    _context.Database.OpenConnection();

                    using (var reader = command.ExecuteReader())
                    {
                        int results = 0;

                        while (reader.Read())
                        {


                            results=((int)reader["total_parking"]);
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

                return BadRequest();
            }
        }

    }
}
