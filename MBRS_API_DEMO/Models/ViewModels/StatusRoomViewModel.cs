using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models.ViewModels
{
    public class StatusRoomViewModel
    {
        [Column("room_id")]
        public int roomID { get; set; }
        [Column("room_name")]
        public string roomName { get; set; }
        [Column("type_room_name")]
        public string typeRoomName { get; set; }
        [Column("full_name")]
        public string fullName { get; set; }
        [Column("check_in")]
        public DateTime checkIn { get; set; }
        [Column("check_out")]
        public DateTime checkOut { get; set; }
        [Column("status_room")]
        public int statusRoom { get; set; }
        [Column("order_id")]
        public int orderID { get; set; }
        [Column("floor_id")]
        public int floorID { get; set; }
        [Column("order_room_detail_id")]
        public int orderRoomDetailID { get; set; }
    }
}
