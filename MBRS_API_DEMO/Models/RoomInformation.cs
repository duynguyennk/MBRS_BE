using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class RoomInformation
    {
        [Column("room_id")]
        public int roomID { get; set; }
        [Column("room_name")]
        public string roomName { get; set; }

        [Column("room_code")]
        public string roomCode { get; set; }

        [Column("type_room_id")]
        public int typeRoomID { get; set; }
        [Column("floor_id")]
        public int floorID { get; set; }
    }
}


