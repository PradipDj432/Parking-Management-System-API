namespace demouserdashboard.Models
{
    public class InUseSlot
    {
        
        public string company_emailid { get; set; }
        public string user_emailid { get; set; }
        public string vehicle_plate { get; set; }
        public int parking_spot_no { get; set; }
        public string entrytime { get; set; }
        public string exittime { get; set; }
        public bool vehicle_type { get; set; }

    }
}
