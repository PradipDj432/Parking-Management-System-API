using System.ComponentModel.DataAnnotations;

namespace demouserdashboard.Models
{
    public class Login
    {
        public string email{ get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }
}
