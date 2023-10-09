using demouserdashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace demouserdashboard.Data
{
    public class UserDbContext: DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<CompanyEdit> company { get; set; }
    }
}
