using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace MBRS_API.Models
{
    public class OrderBike
    {
        [Column("order_id")]
        public int orderID { get; set; }
        [Column("order_code")]
        public string orderCode { get; set; }
        [Column("price")]
        public double price { get; set; }
        [Column("content_payment")]
        public string contentPayment { get; set; }
        [Column("status_payment")]
        public string statusPayment { get; set; }
        [Column("content_result_payment")]
        public string contentResultPayment { get; set; }
        [Column("bank_transaction_number")]
        public string bankTransactionNumber { get; set; }
        [Column("vnp_transaction_number")]
        public string vnpTransactionNumber { get; set; }
        [Column("type_payment")]
        public string typePayment { get; set; }
        [Column("bank_code")]
        public string bankCode { get; set; }
        [Column("date_time")]
        public string dateTime { get; set; }
        [Column("date_time_get_bike")]
        public DateTime dateTimeGetBike { get; set; }
        [Column("date_time_back_bike")]
        public DateTime dateTimeBackBike { get; set; }
        [Column("number_hours_rent")]
        public int numberHoursRent { get; set; }
        [Column("customer_id")]
        public int customerID { get; set; }
        [Column("bike_id")]
        public int bikeID { get; set; }
        [Column("full_name")]
        public string fullName { get; set; }
        [Column("identify_number")]
        public string identifyNumber { get; set; }
        [Column("phone_number")]
        public string phoneNumber { get; set; }
        [Column("email")]
        public string email { get; set; }
        [Column("status_bike")]
        public int statusBike { get; set; }

        public string convertDate
        {
            get
            {
                if (dateTime != null)
                {
                    string format = "yyyyMMddHHmmss";
                    return (DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture)).ToString("dd/MM/yyyy");
                }
                else
                {
                    return "";
                }
            }
        }
        public string convertDateTimeGetBike
        {
            get
            {
                return dateTimeGetBike.ToString("dd/MM/yyyy h:mm:ss tt");
            }
        }
        public string convertDateTimeBackBike
        {
            get
            {
                return dateTimeBackBike.ToString("dd/MM/yyyy h:mm:ss tt");
            }
        }
    }
}
