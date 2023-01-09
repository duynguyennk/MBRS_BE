using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace MBRS_API.Models
{
    public class OrderRoom
    {
        [Column("order_id")]
        public int orderID { get; set; }
        [Column("order_code")]
        public string orderCode { get; set; }
        [Column("check_in")]
        public DateTime checkIn { get; set; }
        [Column("check_out")]
        public DateTime checkOut { get; set; }
        [Column("price")]
        public double price { get; set; }
        [Column("room_id")]
        public int roomID { get; set; }
        [Column("room_name")]
        public string roomName { get; set; }
        [Column("guest_id")]
        public int guestID { get; set; }
        [Column("customer_id")]
        public int CustomerID { get; set; }
        [Column("employee_id")]
        public int employeeID { get; set; }
        [Column("content_payment")]
        public string contentPayment { get; set; }
        [Column("full_name")]
        public string fullName { get; set; }
        [Column("identify_number")]
        public string identityNumber { get; set; }
        [Column("phone_number")]
        public string phoneNumber { get; set; }
        [Column("status_payment")]
        public string statusPayment { get; set; }
        [Column("content_result_payment")]
        public string contentResultPayment { get; set; }
        [Column("bank_transaction_number")]
        public string bankTransactionNumber { get; set; }
        [Column("vnp_transaction_number")]
        public string vpnTransactionNumber { get; set; }
        [Column("type_payment")]
        public string typePayment { get; set; }
        [Column("bank_code")]
        public string bankCode { get; set; }
        [Column("date_time")]
        public string dateTime { get; set; }

        [Column("status_room")]
        public int statusRoom { get; set; }
        [Column("type_order")]
        public int typeOrder { get; set; }
        [Column("delete_flag")]
        public int deleteFlag { get; set; }

        public string statusRoomConvert
        {
            get
            {
                if (statusRoom == 0)
                {
                    return "Phòng trống";
                }
                else if (statusRoom == 1)
                {
                    return "Đã nhận phòng";
                }
                else
                {
                    return "Đã trả phòng";
                }
            }
        }
        public string convertPrice
        {
            get
            {
                if (price != null)
                {

                    return String.Format("{0:n0}", price);
                }
                else
                {
                    return "";
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
