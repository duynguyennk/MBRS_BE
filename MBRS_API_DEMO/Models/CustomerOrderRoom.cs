using MBRS_API_DEMO.Utils;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace MBRS_API.Models
{
    public class CustomerOrderRoom
    {
        [Column("order_id")]
        public int orderID { get; set; }
        [Column("check_in")]
        public DateTime checkIn { get; set; }
        [Column("check_out")]
        public DateTime checkOut { get; set; }
        [Column("order_code")]
        public string orderCode { get; set; }
        [Column("price")]
        public double price { get; set; }
        [Column("status_payment")]
        public string statusPayment { get; set; }
        [Column("date_time")]
        public string dateTime { get; set; }
        [Column("type_room_name")]
        public string typeRoomName { get; set; }
        [Column("number_of_child")]
        public int numberOfChild { get; set; }
        [Column("number_of_bed")]
        public int numberOfBed { get; set; }
        [Column("number_of_view")]
        public int numberOfView { get; set; }
        [Column("number_of_adult")]
        public int numberOfAdult { get; set; }
        [Column("number_of_bath_room")]
        public int numberOfBathRoom { get; set; }
        [Column("total_room")]
        public int totalRoom { get; set; }
        [Column("status_room")]
        public int statusRoom { get; set; }
        [Column("rating_id")]
        public int ratingID { get; set; }
        [Column("type_order")]
        public int typeOrder { get; set; }
        [Column("content_payment")]
        public string contentPayment { get; set; }
        [Column("first_image_base64")]
        public string? firstImageBase64 { get; set; }
        [Column("delete_flag")]
        public int deleteFlag { get; set; }

        public bool checkCancel
        {
            get
            {
                DateTime previousCheckInDate = checkIn.AddDays(-3);
                if (Common.ConvertUTCDateTime() >= previousCheckInDate)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
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
        public string convertDateCheckIn
        {
            get
            {
                return checkIn.ToString("dd/MM/yyyy");
            }
        }
        public string convertDateCheckOut
        {
            get
            {
                return checkOut.ToString("dd/MM/yyyy");
            }
        }
    }
}
