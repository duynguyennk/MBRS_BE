using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class NotificationBell
    {
        [Column("notification_order_receptionist_id")]
        public int notificationOrderReceptionistID { get; set; }
        [Column("content_notification")]
        public string contentNotification { get; set; }
        [Column("status_notification")]
        public Boolean statusNotification { get; set; }
        [Column("type_notification")]
        public int typeNotification { get; set; }
        [Column("date_time")]
        public DateTime dateTime { get; set; }
        public string convertDate
        {
            get
            {
                return dateTime.ToString("dd/MM/yyyy HH:mm");
            }
        }
    }
}
