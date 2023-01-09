using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models.ViewModels
{
    public class RoomViewModel
    {
        [Column("room_id")]
        public int roomID { get; set; }
        [Column("room_code")]
        public string roomCode { get; set; }
        [Column("room_name")]
        public string roomName { get; set; }
        [Column("type_room_name")]
        public string typeRoomName { get; set; }
        [Column("price")]
        public float price { get; set; }
        [Column("room_square")]
        public float roomSquare { get; set; }
        [Column("type_room_id")]
        public float typeRoomID { get; set; }
        [Column("floor_id")]
        public float floorID { get; set; }
    }
}


