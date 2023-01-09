namespace MBRS_API.Models.ViewModels
{
    public class OrderFoodInformationViewModel
    {
        public string orderCode { get; set; }
        public double price { get; set; }
        public string contentPayment { get; set; }
        public string statusPayment { get; set; }
        public string contentResultPayment { get; set; }
        public string bankTransactionNumber { get; set; }
        public string vnpTransactionNumber { get; set; }
        public string typePayment { get; set; }
        public string bankCode { get; set; }
        public string dateTime { get; set; }
        public int quanity { get; set; }
        public int foodID { get; set; }
        public int customerID { get; set; }
        public string foodName { get; set; }
    }
}
