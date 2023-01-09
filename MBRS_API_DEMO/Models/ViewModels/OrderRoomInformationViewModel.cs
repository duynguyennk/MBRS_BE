namespace MBRS_API.Models.ViewModels
{
    public class OrderRoomInformationViewModel
    {
        public string codeOrder { get; set; }
        public double price { get; set; }
        public string contentPayment { get; set; }
        public string resultPayment { get; set; }
        public string contentResultPayment { get; set; }
        public string bankTransactionNumber { get; set; }
        public string vnpTransactionNumber { get; set; }
        public string typePayment { get; set; }
        public string bankCode { get; set; }
        public string timePayment { get; set; }
        public int days { get; set; }
        public string? fullName { get; set; }
        public string? phoneNumber { get; set; }
        public string? email { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string? identifyNumber { get; set; }
        public DateTime checkIn { get; set; }
        public DateTime checkOut { get; set; }
        public int numberOfRoom { get; set; }
        public int typeRoomID { get; set; }
        public int? customerID { get; set; }
    }
}
