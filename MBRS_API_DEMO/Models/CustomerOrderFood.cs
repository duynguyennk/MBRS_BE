using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace MBRS_API.Models
{
    public class CustomerOrderFood
    {
        [Column("order_id")]
        public int orderID { get; set; }
        [Column("order_code")]
        public string orderCode { get; set; }
        [Column("price")]
        public double price { get; set; }
        [Column("status_payment")]
        public string statusPayment { get; set; }
        [Column("content_payment")]
        public string contentPayment { get; set; }
        [Column("date_time")]
        public string dateTime { get; set; }
        [Column("status_food")]
        public int statusFood { get; set; }
        [Column("rating_id")]
        public int ratingID { get; set; }


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
    }
}
