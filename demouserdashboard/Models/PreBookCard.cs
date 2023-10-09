namespace demouserdashboard.Models
{
    public class PreBookCard
    {
        public string user_booking_id { get; set; }
        public string username { get; set; }
        public string company_name { get; set; }
        public string company_address { get; set; }
        public DateTime entrytime { get; set; }
        public DateTime exittime { get; set; }
        public int praking_slot_no { get; set; }
       
    }
}
