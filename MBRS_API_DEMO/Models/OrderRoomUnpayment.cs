namespace MBRS_API.Models
{
    public class OrderRoomUnpayment
    {
        public int typeRoomID { get; set; }
        public string typeRoomName { get; set; }
        public DateTime checkIn { get; set; }
        public DateTime checkOut { get; set; }
        public double price { get; set; }
        public int numberOfRoom { get; set; }
        public int numberOfDay { get; set; }
        public string? fullName { get; set; }
        public string? phoneNumber { get; set; }
        public string? email { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string? identifyNumber { get; set; }
        public int customerID { get; set; }
    }
}
