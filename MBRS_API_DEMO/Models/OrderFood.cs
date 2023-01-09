using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace MBRS_API.Models
{
    public class OrderFood
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
        [Column("vnp_transaction_number")]
        public string vnpTransactionNumber { get; set; }
        [Column("date_time")]
        public string dateTime { get; set; }
        [Column("full_name")]
        public string fullName { get; set; }
        [Column("phone_number")]
        public string phoneNumber { get; set; }
        [Column("status_food")]
        public string statusFood { get; set; }
        public string convertDate
        {
            get
            {
                string format = "yyyyMMddHHmmss";
                return (DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture)).ToString("dd/MM/yyyy");
            }
        }
    }
}
