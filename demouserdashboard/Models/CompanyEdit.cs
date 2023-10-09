using System.ComponentModel.DataAnnotations;

namespace demouserdashboard.Models
{
    public class CompanyEdit
    {
        [Key]
        [EmailAddress]
        public string company_email_id { get; set; }
        [DataType(DataType.Password)]
        public string company_password { get; set; }
        public string company_name { get; set; }
        public string company_contact_no { get; set; }
        public string company_address { get; set; }
        public long? total_parking { get; set; }
        public long? two_wheel_parking { get; set; }
        public long? four_wheel_parking { get; set; }
        public decimal? two_wheel_charge { get; set; }
        public decimal? four_wheel_charge { get; set; }
        public decimal? two_wheel_penalty { get; set; }
        public decimal? four_wheel_penalty { get; set; }
        public string watchman_emailid { get; set; }
        public string watchman_password { get; set; }
    }
}
