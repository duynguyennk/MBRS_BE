namespace MBRS_API.Models.ViewModels
{
    public class OrderBikeInformationViewModel
    {
        public string orderCode { get; set; }
        public int typeBikeID { get; set; }
        public double price { get; set; }
        public string contentPayment { get; set; }
        public string statusPayment { get; set; }
        public string contentResultPayment { get; set; }
        public string bankTransactionNumber { get; set; }
        public string vnpTransactionNumber { get; set; }
        public string typePayment { get; set; }
        public string bankCode { get; set; }
        public string dateTime { get; set; }
        public DateTime dateTimeGetBike { get; set; }
        public int numberHoursRent { get; set; }
        public int numberOfBike { get; set; }
        public string hoursGetBike { get; set; }
        public int customerID { get; set; }
    }
}
